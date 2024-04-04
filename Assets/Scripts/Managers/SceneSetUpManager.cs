using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;
using System.Collections;

public class SceneSetUpManager : MonoBehaviour
{
    public static SceneSetUpManager Instance;
    public static string playMode;

    public static Vector4 IAWeightBot;
    public static Vector4 IAWeightTop;

    public static Partie replay;

    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.SpawnUnits) OnSpawnUnits();
        if (newState == GameState.WaitForPlayer) OnWaitForPlayer();
    }

    private void OnSpawnUnits()
    {
        if (playMode == "Multiplayer" || playMode == "Player vs IA")
        {
            GameObject playerObject = PhotonNetwork.Instantiate("Units/Player", new Vector2(4, 4), Quaternion.identity);
            playerObject.GetComponent<SpriteRenderer>().color = ColorExtension.blue;
        }

        if (playMode == "Player vs IA")
        {
            GameObject IAObject = PhotonNetwork.Instantiate("Units/IANegaAlphaBeta", new Vector2(4, 4), Quaternion.identity);
            IANegaAlphaBeta IA = IAObject.GetComponent<IANegaAlphaBeta>();
            ReferenceManager.Instance.enemy = IA;
            IA.weight = IAWeightTop;
        }

        if (playMode == "IA vs IA")
        {
            GameObject IAObjectBot = PhotonNetwork.Instantiate("Units/IANegaAlphaBeta", new Vector2(4, 4), Quaternion.identity);
            IAObjectBot.GetComponent<SpriteRenderer>().color = ColorExtension.blue;
            IANegaAlphaBeta IABot = IAObjectBot.GetComponent<IANegaAlphaBeta>();
            ReferenceManager.Instance.player = IABot;
            IABot.weight = IAWeightBot;

            GameObject IAObjectTop = PhotonNetwork.Instantiate("Units/IANegaAlphaBeta", new Vector2(4, 4), Quaternion.identity);
            IANegaAlphaBeta IATop = IAObjectTop.GetComponent<IANegaAlphaBeta>();
            ReferenceManager.Instance.enemy = IATop;
            IATop.weight = IAWeightTop;
        }

        if (playMode == "Algo Genetique")
        {
            GameObject IAObjectBot = PhotonNetwork.Instantiate("Units/IANegaAlphaBeta", new Vector2(4, 4), Quaternion.identity);
            IAObjectBot.GetComponent<SpriteRenderer>().color = ColorExtension.blue;
            IANegaAlphaBeta IABot = IAObjectBot.GetComponent<IANegaAlphaBeta>();
            ReferenceManager.Instance.player = IABot;
            IABot.weight = IAWeightBot;

            GameObject IAObjectTop = PhotonNetwork.Instantiate("Units/IANegaAlphaBeta", new Vector2(4, 4), Quaternion.identity);
            IANegaAlphaBeta IATop = IAObjectTop.GetComponent<IANegaAlphaBeta>();
            ReferenceManager.Instance.enemy = IATop;
            IATop.weight = IAWeightTop;
        }

        if (playMode == "Replays")
        {
            GameObject IAObjectBot = PhotonNetwork.Instantiate("Units/IAReplay", new Vector2(4, 4), Quaternion.identity);
            IAObjectBot.GetComponent<SpriteRenderer>().color = ColorExtension.blue;
            IAReplay IAReplayBot = IAObjectBot.GetComponent<IAReplay>();
            ReferenceManager.Instance.player = IAReplayBot;
            IAReplayBot.index = (replay.playerBegins) ? 0 : 1;


            GameObject IAObjectTop = PhotonNetwork.Instantiate("Units/IAReplay", new Vector2(4, 4), Quaternion.identity);
            IAReplay IAReplayTop = IAObjectTop.GetComponent<IAReplay>();
            ReferenceManager.Instance.enemy = IAReplayTop;
            IAReplayTop.index = (replay.playerBegins) ? 1 : 0;
        }

    }

    private void OnWaitForPlayer()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            StartCoroutine(CoinToss(Random.value > 0.5));
        }

        if (PhotonNetwork.OfflineMode)
        {
            if (playMode == "Replays") StartCoroutine(CoinToss(replay.playerBegins));
            else StartCoroutine(CoinToss(Random.value > 0.5));
        }
    }

    private IEnumerator CoinToss(bool toss)
    {
        yield return new WaitForEndOfFrame();
        GameState state = toss ? GameState.Player1Turn : GameState.Player2Turn;
        GameManager.Instance.view.RPC("UpdateGameState", RpcTarget.All, state);
    }
}
