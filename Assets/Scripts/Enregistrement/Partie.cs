using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Partie
{
    public List<Coup> ListCoups = new List<Coup>();
    public bool playerBegins;
    public bool playerWins;

    public override string ToString()
    {
        string s = "";
        foreach (Coup coup in ListCoups) s = s + coup + "\n";
        return s;
    }
}