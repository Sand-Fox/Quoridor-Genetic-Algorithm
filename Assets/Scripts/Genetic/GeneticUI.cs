using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneticUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _tmpIndividuals;
    [SerializeField] private TMP_InputField _tmpGenerations;
    [SerializeField] private TMP_InputField _tmpWinnerProbability;
    [SerializeField] private TMP_InputField _tmpMutateProbability;

    private int _nbIndividuals = 4;
    private int _nbGenerations = 2;
    private int _winnerSelectProbability = 80;
    private int _mutateProbability = 10;

    public void OnIndividualTextChange(string text)
    {
        if (!int.TryParse(text, out int result))
        {
            ResetIndividualText();
            return;
        }

        if (result < 4)
        {
            ResetIndividualText();
            return;
        }

        int log = Mathf.FloorToInt(Mathf.Log(result, 2));
        int finalResult = (int)Mathf.Pow(2, log);

        _nbIndividuals = finalResult;
        _tmpIndividuals.text = finalResult.ToString();
    }

    private void ResetIndividualText()
    {
        _nbIndividuals = 4;
        _tmpIndividuals.text = 4.ToString();
    }

    public void OnGenerationTextChange(string text)
    {
        if (!int.TryParse(text, out int result))
        {
            ResetGenerationText();
            return;
        }

        if (result < 2)
        {
            ResetGenerationText();
            return;
        }

        _nbGenerations = result;
    }

    private void ResetGenerationText()
    {
        _nbGenerations = 2;
        _tmpGenerations.text = 2.ToString();
    }

    public void OnWinnerTextChange(string text)
    {
        if (!int.TryParse(text, out int result))
        {
            ResetWinnerText();
            return;
        }

        if (result < 0 || result > 100)
        {
            ResetWinnerText();
            return;
        }

        _winnerSelectProbability = result;
    }

    private void ResetWinnerText()
    {
        _winnerSelectProbability = 80;
        _tmpWinnerProbability.text = 80.ToString();
    }

    public void OnMutateTextChange(string text)
    {
        if (!int.TryParse(text, out int result))
        {
            ResetMutateText();
            return;
        }

        if (result < 0 || result > 100)
        {
            ResetMutateText();
            return;
        }

        _mutateProbability = result;
    }

    private void ResetMutateText()
    {
        _mutateProbability = 10;
        _tmpMutateProbability.text = 10.ToString();
    }

    public void Play()
    {
        GeneticBase.Instance.StartGeneticAlgorithm(_nbIndividuals, _nbGenerations, _winnerSelectProbability / 100.0f, _mutateProbability / 100.0f);
    }
}
