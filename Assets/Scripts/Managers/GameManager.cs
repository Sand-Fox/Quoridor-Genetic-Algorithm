using System;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PhotonView view;

    public GameState gameState;
    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        UpdateGameState(GameState.GenerateGrid);
        UpdateGameState(GameState.SpawnUnits);
        UpdateGameState(GameState.WaitForPlayer);
    }

    [PunRPC]
    public void UpdateGameState(GameState newState)
    {
        gameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    public bool isPlayerTurn(GameState state = 0)
    {
        if (state == 0) state = gameState;
        if ((PhotonNetwork.LocalPlayer.ActorNumber == 1 && state == GameState.Player1Turn)
            || (PhotonNetwork.LocalPlayer.ActorNumber == 2 && state == GameState.Player2Turn)) return true;

        return false;
    }

    public bool isEnemyTurn(GameState state = 0)
    {
        if (state == 0) state = gameState;
        if ((PhotonNetwork.LocalPlayer.ActorNumber == 1 && state == GameState.Player2Turn)
            || (PhotonNetwork.LocalPlayer.ActorNumber == 2 && state == GameState.Player1Turn)) return true;

        return false;
    }

    public void EndTurn()
    {
        GameState newState = (gameState == GameState.Player1Turn) ? GameState.Player2Turn : GameState.Player1Turn;
        if (ReferenceManager.Instance.player.occupiedTile.transform.position.y == 8) newState = GameState.Win;
        if (ReferenceManager.Instance.enemy.occupiedTile.transform.position.y == 0
            || RegisterManager.Instance.NombreCoups() == 150) newState = GameState.Loose;
        UpdateGameState(newState);
    }

    public void Surrender()
    {
        UpdateGameState(GameState.Loose);
        view.RPC("UpdateGameState", RpcTarget.Others, GameState.Win);
    }
}

public enum GameState
{
    Default = 0,
    GenerateGrid = 1,
    WaitForPlayer = 2,
    SpawnUnits = 3,
    Player1Turn = 4,
    Player2Turn = 5,
    Win = 6,
    Loose = 7
}
