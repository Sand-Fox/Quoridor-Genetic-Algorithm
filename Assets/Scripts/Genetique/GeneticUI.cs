using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneticUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _tmpIndividuals;
    [SerializeField] private TMP_InputField _tmpGenerations;

    private int _nbIndividuals = 4;
    private int _nbGenerations = 2;

    public void OnIndividualTextChange(string text)
    {
        if (int.TryParse(text, out int result))
        {
            int finalResult = Mathf.Max(4, result) - Mathf.Max(4, result) % 4;
            _nbIndividuals = finalResult;
            _tmpIndividuals.text = finalResult.ToString();
        }
        else
        {
            _nbIndividuals = 4;
            _tmpIndividuals.text = 4.ToString();
        }
    }

    public void OnGenerationTextChange(string text)
    {
        if (int.TryParse(text, out int result))
        {
            int finalResult = Mathf.Max(1, result);
            _nbGenerations = finalResult;
            _tmpGenerations.text = finalResult.ToString();
        }
        else
        {
            _nbGenerations = 2;
            _tmpGenerations.text = 2.ToString();
        }
    }

    public void Play()
    {
        Genetic.Instance.StartGeneticAlgorithm(_nbIndividuals, _nbGenerations);
    }
}
