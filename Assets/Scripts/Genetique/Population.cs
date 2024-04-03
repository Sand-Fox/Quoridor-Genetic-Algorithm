using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Population : MonoBehaviour
{
    private List<Vector4> population = new List<Vector4>();
    private List<Vector4> winner = new List<Vector4>();

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
            int finalResult =  Mathf.Max(4, result) - Mathf.Max(4, result) % 4;
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
            population.Add(weight);
        }

        Match();
    }

    private void OnEndGame(bool stateIsWin)
    {
        indexIndividuals += 2;

        IANegaAlphaBeta IAWinner;
        if (stateIsWin) IAWinner = ReferenceManager.Instance.player as IANegaAlphaBeta;
        else IAWinner = ReferenceManager.Instance.enemy as IANegaAlphaBeta;
        winner.Add(IAWinner.weight);

        if (indexIndividuals == nbIndividuals)
        {
            indexGenerations++;
            indexIndividuals = 0;

            if (indexGenerations == nbGenerations)
            {
                string results = "";
                foreach (Vector4 weight in winner) results += weight + "\n";
                UIManager.Instance.EnableGeneticResults(results);
                return;
            }

            population = NewGeneration();
            Match();
            return;
        }

        Match();
    }

    private void Match()
    {
        Vector4 weight1 = population[Random.Range(0, population.Count)];
        population.Remove(weight1);

        Vector4 weight2 = population[Random.Range(0, population.Count)];
        population.Remove(weight2);

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

    private Vector4 Reproduce(Vector4 father, Vector4 mother)
    {
        Vector4 son = new Vector4(
            (Random.value < 0.5) ? father.x : mother.x,
            (Random.value < 0.5) ? father.y : mother.y,
            (Random.value < 0.5) ? father.z : mother.z,
            (Random.value < 0.5) ? father.w : mother.w); 
        return son;
    }

    private Vector4 Modify(Vector4 father, int geneIndex)
    {
        if (geneIndex == 0) return new Vector4(father.x * Random.Range(0.9f, 1.1f), father.y, father.z, father.w);
        if (geneIndex == 1) return new Vector4(father.x, father.y * Random.Range(0.9f, 1.1f), father.z, father.w);
        if (geneIndex == 2) return new Vector4(father.x, father.y, father.z * Random.Range(0.9f, 1.1f), father.w);
        if (geneIndex == 3) return new Vector4(father.x, father.y, father.z, father.w * Random.Range(0.9f, 1.1f));
        Debug.LogWarning("n is not in range of Vector4");
        return default;
    }

    private Vector4 Mutation(Vector4 father)
    {
        Vector4 son = new Vector4();
        int nbGenes = Random.Range(1, 5);

        for(int i = 0; i < nbGenes; i++)
        {
            int geneIndex = Random.Range(0, 4);
            son = Modify(father, geneIndex);
        }
        return son;
    }

    private List<Vector4> NewGeneration()
    {
        List<Vector4> newGen = new List<Vector4>();
        while(winner.Count > 0)
        {
            Vector4 father = winner[Random.Range(0, winner.Count)];
            winner.Remove(father);

            Vector4 mother = winner[Random.Range(0, winner.Count)];
            winner.Remove(mother);

            newGen.Add(father);
            newGen.Add(mother);

            Vector4 child1 = Reproduce(father, mother);
            Vector4 child2 = Reproduce(father, mother);


            if (Random.value < 0.01) child1 = Mutation(child1);
            if (Random.value < 0.01) child2 = Mutation(child2);
            
            newGen.Add(child1);
            newGen.Add(child2);
        }
        return newGen;
    }
}

