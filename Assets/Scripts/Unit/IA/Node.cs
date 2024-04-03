using System.Collections.Generic;

public class Node
{
    public static readonly int initialScore = 10000;

    public float score;
    public int depth;
    public Coup coup;

    public Node(Coup _coup, int _depth)
    {
        coup = _coup;
        depth = _depth;
        score = initialScore;
    }
}
