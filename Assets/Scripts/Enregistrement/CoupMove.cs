using UnityEngine;

[System.Serializable]
public class CoupMove : Coup
{
    public CoupMove(Vector2 _coord) : base(_coord) { }

    public override string ToString()
    {
        return "Move : " + "Coordonnées (" + coord[0] + ", " + coord[1] + ")";
    }
}
