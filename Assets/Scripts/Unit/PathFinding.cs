using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static PathFinding Instance;

    private void Awake() => Instance = this;

    private List<CustomTile> open;
    private List<CustomTile> closed;

    public List<CustomTile> GetWiningPath(BaseUnit unit)
    {
        int endRawIndex;
        if (unit == ReferenceManager.Instance.player) endRawIndex = GridManager.BOUNDS - 1;
        else endRawIndex = 0;

        if (unit.occupiedTile.transform.position.y == endRawIndex) return new List<CustomTile>();

        CustomTile targetTile = SetUpPath(unit, endRawIndex);
        if (targetTile == null) return null;

        List<CustomTile> path = new List<CustomTile>();
        CustomTile currentTile = targetTile;

        while (currentTile != unit.occupiedTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.previousTile;
        }
        path.Reverse();
        return path;
    }

    private CustomTile SetUpPath(BaseUnit unit, int endRawIndex)
    {
        open = new List<CustomTile>();
        closed = new List<CustomTile>();

        CustomTile unitTile = unit.occupiedTile;
        unitTile.previousTile = null;
        unitTile.G = 0;
        unitTile.H = DistanceToEndRaw(unitTile, endRawIndex, unit);
        open.Add(unitTile);

        while (true)
        {
            CustomTile current = GetLowestFCostInOpen();
            // S'il n'existe pas de chemin vers le but
            if (current == null) return null; 

            open.Remove(current);
            closed.Add(current);
            if(current.transform.position.y == endRawIndex) return current;

            foreach (CustomTile neighbour in current.AdjacentTiles())
            {
                if (closed.Contains(neighbour)) continue;

                bool neighbourIsFresh = !open.Contains(neighbour);
                if (neighbourIsFresh || current.G + 1 < neighbour.G)
                {
                    neighbour.previousTile = current;
                    neighbour.G = current.G + 1;
                    neighbour.H = DistanceToEndRaw(neighbour, endRawIndex, unit);
                    if (neighbourIsFresh) open.Add(neighbour);
                }
            }
        }
    }

    private int DistanceToEndRaw(CustomTile source, int endRawIndex, BaseUnit myUnit)
    {
        Vector2 sourcePosition = source.transform.position;
        int distance = Mathf.Abs(endRawIndex - (int)sourcePosition.y);
        Vector2 otherUnitPosition = myUnit.OtherUnit().occupiedTile.transform.position;

        if (sourcePosition.x == otherUnitPosition.x &&
            otherUnitPosition.y.IsBetween(sourcePosition.y, endRawIndex)) distance--;

        return distance;
    }

    private CustomTile GetLowestFCostInOpen()
    {
        if(open.Count == 0) return null;

        CustomTile bestTile = open[0];
        foreach(CustomTile tile in open)
        {
            if (tile.F < bestTile.F) bestTile = tile;
            if (tile.F == bestTile.F && tile.H < bestTile.H) bestTile = tile;
        }
        return bestTile;
    }
}