using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public abstract class Genetic : MonoBehaviour
{
    private List<Vector4> _population = new();

    private int nbIndividuals = 4;
    private int nbGenerations = 2;

    private int indexIndividuals;
    private int indexGenerations;

    [SerializeField] private TMP_InputField TmpIndividuals;
    [SerializeField] private TMP_InputField TmpGenerations;

    private void Awake()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
        DontDestroyOnLoad(gameObject);
    }

    public void OnIndividualTextChange(string text)
    {
        if (int.TryParse(text, out int result))
        {
            int finalResult = Mathf.Max(4, result) - Mathf.Max(4, result) % 4;
            nbIndividuals = finalResult;
            TmpIndividuals.text = finalResult.ToString();
        }
        else
        {
            nbIndividuals = 4;
            TmpIndividuals.text = 4.ToString();
        }
    }

    public void OnGenerationTextChange(string text)
    {
        if (int.TryParse(text, out int result))
        {
            int finalResult = Mathf.Max(1, result);
            nbGenerations = finalResult;
            TmpGenerations.text = finalResult.ToString();
        }
        else
        {
            nbGenerations = 2;
            TmpGenerations.text = 2.ToString();
        }
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Win && SceneSetUpManager.playMode == "Algo Genetique") OnEndGame(true);
        if (newState == GameState.Loose && SceneSetUpManager.playMode == "Algo Genetique") OnEndGame(false);
    }

    public void Play()
    {
        indexIndividuals = 0;
        indexGenerations = 0;

        for (int i = 0; i < nbIndividuals; i++)
        {
            Vector4 weight = new Vector4(Random.value, Random.value, Random.value, Random.value);
            _population.Add(weight);
        }

        Match();
    }

    private void Match()
    {
        Vector4 weight1 = _population.GetRandom();
        _population.Remove(weight1);

        Vector4 weight2 = _population.GetRandom();
        _population.Remove(weight2);

        SceneSetUpManager.IAWeight1 = weight1;
        SceneSetUpManager.IAWeight2 = weight2;

        StartCoroutine(ReloadGame());
    }

    private IEnumerator ReloadGame()
    {
        yield return new WaitForSeconds(BaseUnit.movementDuration);
        if (PhotonNetwork.InRoom) PhotonNetwork.LoadLevel("Game");
        else PrivateRoom.Instance.CreatePrivateRoom();
    }

    private void OnEndGame(bool stateIsWin)
    {
        indexIndividuals += 2;

        IANegaAlphaBeta IAWinner;
        IANegaAlphaBeta IALooser;

        if (stateIsWin)
        {
            IAWinner = ReferenceManager.Instance.player as IANegaAlphaBeta;
            IALooser = ReferenceManager.Instance.enemy as IANegaAlphaBeta;
        }
        else
        {
            IAWinner = ReferenceManager.Instance.enemy as IANegaAlphaBeta;
            IALooser = ReferenceManager.Instance.player as IANegaAlphaBeta;
        }

        _winners.Add(IAWinner.weight);
        _loosers.Add(IALooser.weight);

        if (indexIndividuals == nbIndividuals)
        {
            indexGenerations++;
            indexIndividuals = 0;

            if (indexGenerations == nbGenerations)
            {
                string results = "";
                foreach (Vector4 weight in _winners) results += weight + "\n";
                UIManager.Instance.EnableGeneticResults(results);
                return;
            }

            _population = CreateNewGeneration();
            Match();
            return;
        }

        Match();
    }

    /// <summary>
    /// Populate the next generation before creating matchs.
    /// </summary>
    protected abstract List<Vector4> CreateNewGeneration();

    /// <summary>
    /// The winners of the last generation.
    /// </summary>
    protected List<Vector4> _winners = new();

    /// <summary>
    /// The loosers of the last generation.
    /// </summary>
    protected List<Vector4> _loosers = new();
}
