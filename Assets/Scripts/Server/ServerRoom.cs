using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class ServerRoom : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public void CreateServerRoom()
    {
        if (createInput.text == "") return;
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinServerRoom()
    {
        if (joinInput.text == "") return;
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        SceneSetUpManager.playMode = "Multiplayer";
        PhotonNetwork.LoadLevel("Game");
    }
}
