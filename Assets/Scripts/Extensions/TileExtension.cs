using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileExtension
{
    public static Vector2 DirectionTo(this CustomTile source, CustomTile destination)
    {
        return destination.transform.position - source.transform.position;
    }

    public static Vector2 ReflectPosition(this Vector3 position)
    {
        Vector2 ret = Vector2.Reflect(position, new Vector2(0, 1));
        return ret + new Vector2(0, 8);
    }

    public static CustomTile GetTileInDirection(this CustomTile centerTile, Vector2 direction, out CustomTile tile2)
    {
        tile2 = null;
        Vector2 centerTilePos = centerTile.transform.position;
        CustomTile adjacentTile = GridManager.Instance.GetTileAtPosition(centerTilePos + direction);
        if (!centerTile.directionDico[direction] || adjacentTile == null) return null;

        if (adjacentTile.occupiedUnit == null) return adjacentTile;
        if (adjacentTile.directionDico[direction])
        {
            CustomTile furtherTile = GridManager.Instance.GetTileAtPosition(centerTilePos + 2 * direction);
            if (furtherTile != null) return furtherTile;
        }
        tile2 = adjacentTile.GetTileInDirection(Vector2.Perpendicular(direction), out _);
        return adjacentTile.GetTileInDirection(-Vector2.Perpendicular(direction), out _);
    }

    public static CustomTile[] AdjacentTiles(this CustomTile centerTile)
    {
        List<CustomTile> list = new List<CustomTile>();
        CustomTile tileA, tileB, tileC, tileD;

        CustomTile tile1 = centerTile.GetTileInDirection(Vector2.right, out tileA);
        CustomTile tile2 = centerTile.GetTileInDirection(Vector2.left, out tileB);
        CustomTile tile3 = centerTile.GetTileInDirection(Vector2.up, out tileC);
        CustomTile tile4 = centerTile.GetTileInDirection(Vector2.down, out tileD);

        if (tile3 != null) list.Add(tile3);
        if (tile4 != null) list.Add(tile4);
        if (tile1 != null) list.Add(tile1);
        if (tile2 != null) list.Add(tile2);
        
        if (tileC != null) list.Add(tileC);
        if (tileD != null) list.Add(tileD);
        if (tileA != null) list.Add(tileA);
        if (tileB != null) list.Add(tileB);
        

        return list.ToArray();
    }
}
