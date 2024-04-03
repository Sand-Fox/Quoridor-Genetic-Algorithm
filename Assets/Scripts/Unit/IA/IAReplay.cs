using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAReplay : BaseIA
{
    public int index;

    protected override void PlayIA()
    {
        if (index >= SceneSetUpManager.replay.ListCoups.Count)
        {
            if (SceneSetUpManager.replay.playerWins) GameManager.Instance.UpdateGameState(GameState.Win);
            else GameManager.Instance.UpdateGameState(GameState.Loose);
            return;
        }

        Coup coup = SceneSetUpManager.replay.ListCoups[index];

        if (coup is CoupWall coupWall)
        {
            Vector3 wallPosition = new Vector3(coupWall.coord[0], coupWall.coord[1], 0);
            Orientation orientation = coupWall.orientation;
            SpawnWall(wallPosition, orientation);
        }

        if (coup is CoupMove coupMove)
        {
            Vector3 position = new Vector3(coupMove.coord[0], coupMove.coord[1], 0);
            SetUnit(position);
        }

        index += 2;
    }
}
