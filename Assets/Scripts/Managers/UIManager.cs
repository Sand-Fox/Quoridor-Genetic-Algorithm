using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject cornersFolder;
    [SerializeField] private ButtonScript pathButton;
    [SerializeField] private ButtonScript moveButton;
    [SerializeField] private ButtonScript wallButton;
    [SerializeField] private GameObject banner;
    [SerializeField] private PanelScript optionsPanel;
    [SerializeField] private PanelScript winPanel;
    [SerializeField] private PanelScript loosePanel;
    [SerializeField] private PanelScript GeneticResultsPanel;
    [SerializeField] private TextMeshProUGUI GeneticResultsTmp;

    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += OnGameStateChanged;

        if(SceneSetUpManager.playMode == "Replays")
        {
            winPanel.DisableSaveButton();
            loosePanel.DisableSaveButton();
        }
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Player1Turn || newState == GameState.Player2Turn) banner.SetActive(false);

        if (SceneSetUpManager.playMode != "IA vs IA" && GameManager.Instance.isPlayerTurn(newState))
            Invoke(nameof(EnableBluttons), BaseUnit.movementDuration);

        else
        {
            pathButton.EnableButton(false);
            moveButton.EnableButton(false);
            wallButton.EnableButton(false);
        }

        if(newState == GameState.Loose)
        {
            if (optionsPanel.isEnable) optionsPanel.EnablePanel(false);
            loosePanel.EnablePanel(true);
        }
        if (newState == GameState.Win)
        {
            if (optionsPanel.isEnable) optionsPanel.EnablePanel(false);
            winPanel.EnablePanel(true);
        }
    }

    private void EnableBluttons()
    {
        pathButton.EnableButton(true);
        moveButton.EnableButton(true);
        wallButton.EnableButton(ReferenceManager.Instance.player.wallCount > 0);
    }

    public void UpdateWallCountText()
    {
        wallButton.ChangeMainText("Wall (<blue>" + ReferenceManager.Instance.player.wallCount + "</blue> - <red>" + ReferenceManager.Instance.enemy.wallCount + "</red>)");
    }

    public void EnableGeneticResults(string text)
    {
        GeneticResultsTmp.text = text;
        GeneticResultsPanel.EnablePanel(true);
    }
}
