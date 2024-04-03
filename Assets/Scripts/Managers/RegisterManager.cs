using System.Collections.Generic;
using UnityEngine;
using System;

public class RegisterManager : MonoBehaviour
{
    public static RegisterManager Instance;
    private Partie partie  = new Partie();

    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Win) partie.playerWins = true;
        if (newState == GameState.Loose) partie.playerWins = false;
    }

    public void AddCoup(Coup c)
    {
        if (partie.ListCoups.Count == 0)
            partie.playerBegins = GameManager.Instance.isPlayerTurn();

        partie.ListCoups.Add(c);
    }

    public void SavePartie()
    {
        string day = (DateTime.Now.Day <= 9) ? "0" + DateTime.Now.Day : DateTime.Now.Day.ToString();
        string month = (DateTime.Now.Month <= 9) ? "0" + DateTime.Now.Month : DateTime.Now.Month.ToString();
        string hour = (DateTime.Now.Hour <= 9) ? "0" + DateTime.Now.Hour : DateTime.Now.Hour.ToString();
        string minute = (DateTime.Now.Minute <= 9) ? "0" + DateTime.Now.Minute : DateTime.Now.Minute.ToString();
        string second = (DateTime.Now.Second <= 9) ? "0" + DateTime.Now.Second : DateTime.Now.Second.ToString();
        SaveSystem.Save(partie, day + "-" + month + "_" + hour + "-" + minute + "-" + second);
    }

    public int NombreCoups() => partie.ListCoups.Count;
}
