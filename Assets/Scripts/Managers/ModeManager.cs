using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public static ModeManager Instance;

    [SerializeField] private VoidEventChannelSO destropPopUpEvent;
    public Mode mode = Mode.Normal;
  
    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Player1Turn || newState == GameState.Player2Turn) UpdateMode(Mode.Normal);
    }

    public void UpdateMode(Mode newMode)
    {
        mode = newMode;
        
        if (newMode == Mode.Move) EnablePlayerMoveZone();
        else DisablePlayerMoveZone();

        EnableCornerVisual(newMode == Mode.Wall);

        if (newMode == Mode.PathFinding) DrawPathFindingPopUps();
        else destropPopUpEvent.RaiseEvent();
    }

    public void UpdateModeFromButton(int newMode)
    {
        UpdateMode((Mode)newMode);
    }

    // Mode Move
    private void EnablePlayerMoveZone()
    {
        CustomTile playerTile = ReferenceManager.Instance.player.occupiedTile;
        foreach (CustomTile tile in playerTile.AdjacentTiles())
        {
            tile.EnableVisual(true);
        }
    }

    private void DisablePlayerMoveZone()
    {
        foreach (KeyValuePair<Vector2, CustomTile> pair in GridManager.Instance.tilesDico)
        {
            pair.Value.EnableVisual(false);
        }
    }

    // Mode Wall
    private void EnableCornerVisual(bool enable)
    {
        foreach (KeyValuePair<Vector2, CustomCorner> pair in GridManager.Instance.cornersDico)
        {
            pair.Value.EnableVisual(enable);
        }
    }

    // Mode PathFinding
    private void DrawPathFindingPopUps()
    {
        BaseUnit player = ReferenceManager.Instance.player;
        List<CustomTile> playerWiningPath = PathFinding.Instance.GetWiningPath(player);
        if (playerWiningPath != null)
        {
            playerWiningPath.Insert(0, player.occupiedTile);
            LinePopUp.Create(playerWiningPath, ColorExtension.blue);
        }

        BaseUnit enemy = ReferenceManager.Instance.enemy;
        List<CustomTile> enemyWiningPath = PathFinding.Instance.GetWiningPath(enemy);
        if (enemyWiningPath != null)
        {
            enemyWiningPath.Insert(0, enemy.occupiedTile);
            LinePopUp.Create(enemyWiningPath, ColorExtension.red);
        }
    }
}

public enum Mode
{
    Normal = 0,
    Move = 1,
    Wall = 2,
    PathFinding = 3
}
