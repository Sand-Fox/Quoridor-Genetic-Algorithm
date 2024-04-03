using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMove : BaseIA
{
    public static string description = "AI that moves using A* algorithm";

    protected override void PlayIA()
    {
        List<CustomTile> path = PathFinding.Instance.GetWiningPath(this);
        if (path != null) SetUnit(path[0].transform.position);
        else SetUnit(occupiedTile.AdjacentTiles()[0].transform.position);
    }
}
