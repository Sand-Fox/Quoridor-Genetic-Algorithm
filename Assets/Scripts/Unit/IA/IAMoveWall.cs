using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IAMoveWall : BaseIA
{
    public static string description = "AI that randomly chooses between\nputting up a wall and moving";

    protected override void PlayIA()
    {
        if (wallCount > 0 && Random.value > 0.5)
        {
            Vector3 wallPosition = GetBestWallPosition(out Orientation orientation);
            if (wallPosition != default)
            {
                SpawnWall(wallPosition, orientation);
                return;
            }
        }

        List<CustomTile> path = PathFinding.Instance.GetWiningPath(this);
        if (path != null) SetUnit(path[0].transform.position);
        else SetUnit(occupiedTile.AdjacentTiles()[0].transform.position);
    }

    private Vector2 GetBestWallPosition(out Orientation bestOrientation)
    {
        List<CustomTile> playerBestPath = PathFinding.Instance.GetWiningPath(OtherUnit());
        playerBestPath.Insert(0, OtherUnit().occupiedTile);

        Vector2 bestWallPosition = default; bestOrientation = default;
        int longerPathCount = 0;

        for (int i = 0; i < playerBestPath.Count - 1; i++)
        {
            CustomTile currentTile = playerBestPath[i];
            CustomTile nextTile = playerBestPath[i + 1];
            Vector2 direction = nextTile.transform.position - currentTile.transform.position;

            CustomCorner corner1 = GridManager.Instance.GetCornerAtPosition((Vector2)currentTile.transform.position + 0.5f * direction + 0.5f * Vector2.Perpendicular(direction));
            CustomCorner corner2 = GridManager.Instance.GetCornerAtPosition((Vector2)currentTile.transform.position + 0.5f * direction - 0.5f * Vector2.Perpendicular(direction));

            if (corner1 != null)
            {
                Orientation orientation;
                if (direction == Vector2.up || direction == Vector2.down) orientation = Orientation.Horizontal;
                else if (direction == Vector2.right || direction == Vector2.left) orientation = Orientation.Vertical;
                else continue;

                bool canSpawnHere = (orientation == Orientation.Horizontal) ? HorizontalWall.CanSpawnHere(corner1) : VerticalWall.CanSpawnHere(corner1);
                if (canSpawnHere)
                {
                    SpawnWallWhenTesting(corner1.transform.position, orientation);
                    List<CustomTile> pathAfterWall = PathFinding.Instance.GetWiningPath(OtherUnit());
                    if (pathAfterWall.Count > longerPathCount)
                    {
                        bestWallPosition = corner1.transform.position;
                        bestOrientation = orientation;
                        longerPathCount = pathAfterWall.Count;
                    }
                    DespawnWallWhenTesting(corner1.transform.position, orientation);
                }
            }

            if (corner2 != null)
            {
                Orientation orientation;
                if (direction == Vector2.up || direction == Vector2.down) orientation = Orientation.Horizontal;
                else if (direction == Vector2.right || direction == Vector2.left) orientation = Orientation.Vertical;
                else continue;

                bool canSpawnHere = (orientation == Orientation.Horizontal) ? HorizontalWall.CanSpawnHere(corner2) : VerticalWall.CanSpawnHere(corner2);
                if (canSpawnHere)
                {
                    SpawnWallWhenTesting(corner2.transform.position, orientation);
                    List<CustomTile> pathAfterWall = PathFinding.Instance.GetWiningPath(OtherUnit());
                    if (pathAfterWall.Count > longerPathCount)
                    {
                        bestWallPosition = corner2.transform.position;
                        bestOrientation = orientation;
                        longerPathCount = pathAfterWall.Count;
                    }
                    DespawnWallWhenTesting(corner2.transform.position, orientation);
                }
            }
        }

        return bestWallPosition;
    }

}
