using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LeaveRoom : MonoBehaviourPunCallbacks
{
    public static LeaveRoom Instance;
    private void Awake() => Instance = this;

    public void LeaveCurrentRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(SceneSetUpManager.playMode);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        if (GameManager.Instance.gameState != GameState.Loose && GameManager.Instance.gameState != GameState.Win)
            GameManager.Instance.UpdateGameState(GameState.Win);
    }
}
