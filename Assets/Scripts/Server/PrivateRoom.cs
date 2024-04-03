using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PrivateRoom : MonoBehaviourPunCallbacks
{
    public static PrivateRoom Instance;
    private void Awake() => Instance = this;

    public void CreatePrivateRoom()
    {
        if(PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();
        else
        {
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.CreateRoom("PrivateRoom");
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.CreateRoom("PrivateRoom");
    }

    public override void OnJoinedRoom()
    {
        SceneSetUpManager.playMode = SceneManager.GetActiveScene().name;
        PhotonNetwork.LoadLevel("Game");
    }
}
