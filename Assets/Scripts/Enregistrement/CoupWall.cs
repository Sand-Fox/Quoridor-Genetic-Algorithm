using UnityEngine;

[System.Serializable]
public class CoupWall : Coup
{
    public Orientation orientation;

    public CoupWall(Vector2 _coord, Orientation _orientation) : base(_coord)
    {
        orientation = _orientation;
    }

    public override string ToString()
    {
        return "Wall : " + "Coordonnées (" + coord[0] + ", " + coord[1] + "), Orientation (" + orientation + ")";
    }
}
