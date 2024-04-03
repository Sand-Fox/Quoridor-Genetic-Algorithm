using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public abstract class Genetic : MonoBehaviour
{
    public static Genetic Instance;

    private List<Vector4> _population = new();

    private int _nbIndividuals;
    private int _nbGenerations;

    private int _indexIndividuals;
    private int _indexGenerations;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }


    // ------------------------------------------ Handle Matchs ------------------------------------------


    public void StartGeneticAlgorithm(int nbIndividuals, int nbGenerations)
    {
        _nbIndividuals = nbIndividuals;
        _nbGenerations = nbGenerations;
        _indexIndividuals = 0;
        _indexGenerations = 0;

        for (int i = 0; i < _nbIndividuals; i++)
        {
            Vector4 weight = new Vector4(Random.value, Random.value, Random.value, Random.value);
            _population.Add(weight);
        }

        Match();
    }

    private void Match()
    {
        Vector4 weight1 = _population.GetRandom();
        Vector4 weight2 = _population.GetRandom();

        _population.Remove(weight1);
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


    // ------------------------------------------ On End Match ------------------------------------------


    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Win && SceneSetUpManager.playMode == "Algo Genetique") OnEndMatch(true);
        if (newState == GameState.Loose && SceneSetUpManager.playMode == "Algo Genetique") OnEndMatch(false);
    }

    private void OnEndMatch(bool stateIsWin)
    {
        _indexIndividuals += 2;

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

        if (_indexIndividuals == _nbIndividuals)
        {
            _indexGenerations++;
            _indexIndividuals = 0;

            if (_indexGenerations == _nbGenerations)
            {
                string results = "";
                foreach (Vector4 weight in _winners) results += weight + "\n";
                UIManager.Instance.EnableGeneticResults(results);
                return;
            }

            _population = CreateNewGeneration();

            if (_population.Count != _nbIndividuals)
            {
                Debug.LogError("Attention : Toutes les generations doivent avoir le meme nombre d'individus.");
            }

            Match();
            return;
        }

        Match();
    }


    // ------------------------------------------ Protected Properties ------------------------------------------


    /// <summary>
    /// Populate the next generation before creating matchs.
    /// Warning : The next generation must have the same number of individuals as the current generation.
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
