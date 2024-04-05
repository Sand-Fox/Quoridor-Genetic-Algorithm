using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public abstract class GeneticBase : MonoBehaviour
{
    public static GeneticBase Instance;

    private List<Vector4> _population = new();

    protected int _nbIndividuals;
    protected int _nbGenerations;
    protected float _winnerSelectProbability;
    protected float _mutateProbability;

    private int _indexGenerations;
    private bool _isComplete;

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


    private void Match()
    {
        Vector4 weight1 = _population.GetRandom();
        Vector4 weight2 = _population.GetRandom();

        _population.Remove(weight1);
        _population.Remove(weight2);

        SceneSetUpManager.IAWeightBot = weight1;
        SceneSetUpManager.IAWeightTop = weight2;

        StartCoroutine(ReloadGame());
    }

    private IEnumerator ReloadGame()
    {
        yield return new WaitForSeconds(BaseUnit.movementDuration);
        if (PhotonNetwork.InRoom) PhotonNetwork.LoadLevel("Game");
        else PrivateRoom.Instance.CreatePrivateRoom();
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Win && SceneSetUpManager.playMode == "Algo Genetique") OnEndMatch(true);
        if (newState == GameState.Loose && SceneSetUpManager.playMode == "Algo Genetique") OnEndMatch(false);
    }

    private void OnEndMatch(bool stateIsWin)
    {
        var IABot = ReferenceManager.Instance.player as IANegaAlphaBeta;
        var IATop = ReferenceManager.Instance.enemy as IANegaAlphaBeta;

        _winners.Add(stateIsWin ? IABot.weight : IATop.weight);
        _loosers.Add(stateIsWin ? IATop.weight : IABot.weight);

        if (_population.Count != 0)
        {
            Match();
            return;
        }

        if (_isComplete) NewTournamentGeneration();
        else NewGeneticGeneration();
    }


    // ------------------------------------------ Genetic ------------------------------------------


    public void StartGeneticAlgorithm(int nbIndividuals, int nbGenerations, float winnerSelectProbability, float mutateProbability)
    {
        _nbIndividuals = nbIndividuals;
        _nbGenerations = nbGenerations;
        _winnerSelectProbability = winnerSelectProbability;
        _mutateProbability = mutateProbability;

        _indexGenerations = 0;
        _isComplete = false;

        for (int i = 0; i < _nbIndividuals; i++)
        {
            Vector4 weight = new Vector4(Random.value, Random.value, Random.value, Random.value);
            _population.Add(weight);
        }

        Match();
    }

    private void NewGeneticGeneration()
    {
        _indexGenerations++;

        if (_indexGenerations == _nbGenerations)
        {
            _isComplete = true;
            NewTournamentGeneration();
            return;
        }

        _population = CreateNewGeneration();
        _winners.Clear();
        _loosers.Clear();

        if (_population.Count != _nbIndividuals)
        {
            Debug.LogError("Attention : Toutes les generations doivent avoir le meme nombre d'individus.");
        }

        Match();
    }


    // ------------------------------------------ Tournament ------------------------------------------


    private void NewTournamentGeneration()
    {
        if (_winners.Count == 1)
        {
            string results = _winners[0].ToString();
            UIManager.Instance.EnableGeneticResults(results);
            return;
        }

        _population = new(_winners);
        _winners.Clear();
        _loosers.Clear();

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
