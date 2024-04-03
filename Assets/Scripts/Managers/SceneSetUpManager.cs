using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;
using System.Collections;

public class SceneSetUpManager : MonoBehaviour
{
    public static SceneSetUpManager Instance;
    public static string playMode;

    public static string IAName1;
    public static string IAName2;

    public static Vector4 IAWeight1;
    public static Vector4 IAWeight2;

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
            GameObject IAObject = PhotonNetwork.Instantiate(IAName1, new Vector2(4, 4), Quaternion.identity);
            BaseIA IA = IAObject.GetComponent<BaseIA>();
            ReferenceManager.Instance.enemy = IA;
        }

        if (playMode == "IA vs IA")
        {
            GameObject IAObject1 = PhotonNetwork.Instantiate(IAName1, new Vector2(4, 4), Quaternion.identity);
            IAObject1.GetComponent<SpriteRenderer>().color = ColorExtension.blue;
            ReferenceManager.Instance.player = IAObject1.GetComponent<BaseIA>();

            GameObject IAObject2 = PhotonNetwork.Instantiate(IAName2, new Vector2(4, 4), Quaternion.identity);
            ReferenceManager.Instance.enemy = IAObject2.GetComponent<BaseIA>();
        }

        if (playMode == "Algo Genetique")
        {
            GameObject IAObject1 = PhotonNetwork.Instantiate("Units/IANegaAlphaBeta", new Vector2(4, 4), Quaternion.identity);
            IAObject1.GetComponent<SpriteRenderer>().color = ColorExtension.blue;
            IANegaAlphaBeta IA1 = IAObject1.GetComponent<IANegaAlphaBeta>();
            ReferenceManager.Instance.player = IA1;
            IA1.weight = IAWeight1;

            GameObject IAObject2 = PhotonNetwork.Instantiate("Units/IANegaAlphaBeta", new Vector2(4, 4), Quaternion.identity);
            IANegaAlphaBeta IA2 = IAObject2.GetComponent<IANegaAlphaBeta>();
            ReferenceManager.Instance.enemy = IA2;
            IA2.weight = IAWeight2;
        }

        if (playMode == "Replays")
        {
            GameObject IAObject1 = PhotonNetwork.Instantiate("Units/IAReplay", new Vector2(4, 4), Quaternion.identity);
            IAObject1.GetComponent<SpriteRenderer>().color = ColorExtension.blue;
            IAReplay IAReplay1 = IAObject1.GetComponent<IAReplay>();
            ReferenceManager.Instance.player = IAReplay1;
            IAReplay1.index = (replay.playerBegins) ? 0 : 1;


            GameObject IAObject2 = PhotonNetwork.Instantiate("Units/IAReplay", new Vector2(4, 4), Quaternion.identity);
            IAReplay IAReplay2 = IAObject2.GetComponent<IAReplay>();
            ReferenceManager.Instance.enemy = IAReplay2;
            IAReplay2.index = (replay.playerBegins) ? 1 : 0;
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
