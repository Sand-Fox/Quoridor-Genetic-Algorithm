using UnityEngine;

[System.Serializable]
public abstract class Coup
{
    public float[] coord;

    public Coup(Vector2 _coord)
    {
        coord = new float[2];
        coord[0] = _coord.x;
        coord[1] = _coord.y;
    }
}