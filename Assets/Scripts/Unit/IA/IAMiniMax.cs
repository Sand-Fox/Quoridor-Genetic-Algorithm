using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class IAMiniMax : BaseIA
{
    public static string description = "AI that chooses the best move using\nMini Max algorithm (not recommended)";
    public Vector4 weight = new Vector4(1, 1, 1, 1);
    public int defaultDepth = 2;

    protected override void PlayIA()
    {
        List<CustomTile> pathIA = PathFinding.Instance.GetWiningPath(this);
        List<CustomTile> pathP = PathFinding.Instance.GetWiningPath(OtherUnit());

        if (pathIA == null)
        {
            Debug.Log("Pas de meiileur chemin trouvé");
            SetUnit(occupiedTile.AdjacentTiles()[0].transform.position);
            return;
        }

        if (pathP == null || wallCount == 0)
        {
            SetUnit(pathIA[0].transform.position);
            return;
        }

        Node node = new Node(null, 0);
        Coup coup = Max(node, defaultDepth);

        if (coup is CoupWall coupWall)
        {
            Vector3 wallPosition = new Vector3(coupWall.coord[0], coupWall.coord[1], 0);
            Orientation orientation = coupWall.orientation;
            SpawnWall(wallPosition, orientation);
        }

        if(coup is CoupMove coupMove)
        {
            Vector3 position = new Vector3(coupMove.coord[0], coupMove.coord[1], 0);
            SetUnit(position);
        }
    }

    private float CalculScore()
    {
        List<CustomTile> pathIA = PathFinding.Instance.GetWiningPath(this);
        List<CustomTile> pathP = PathFinding.Instance.GetWiningPath(OtherUnit());

        if (pathIA == null || pathP == null)
        {
            Debug.Log("Ce chemin est bloquant");
            return 0;
        }

        int nbWallIA = wallCount;
        int nbWallP = OtherUnit().wallCount;

        float score = weight.x * pathP.Count - weight.y * pathIA.Count - weight.z * nbWallP + weight.w * nbWallIA;
        //float score = weight.x * distP;
        //float score = -weight.y * distIA;
        return score;
    }

    private Coup Max(Node current, int maxDepth)
    {
        if (current.depth == maxDepth)
        {
            current.score = CalculScore();
            return current.coup;
        }

        Coup bestCoup = null;

        if (wallCount > 0)
        {
            foreach(KeyValuePair < Vector2, CustomCorner > pair in GridManager.Instance.cornersDico)
            {
                if (HorizontalWall.CanSpawnHere(pair.Value))
                {
                    SpawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Horizontal);
                    Node node = new Node(coupWall, current.depth + 1);

                    Min(node, maxDepth);

                    if (current.score < node.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    DespawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                }
            
                if (VerticalWall.CanSpawnHere(pair.Value))
                {
                    SpawnWallWhenTesting(pair.Key, Orientation.Vertical);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Vertical);
                    Node node = new Node(coupWall, current.depth + 1);

                    Min(node, maxDepth);

                    if (current.score < node.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    DespawnWallWhenTesting(pair.Key, Orientation.Vertical);
                }
            }
        }

        CustomTile IATile = occupiedTile;

        foreach (CustomTile tile in IATile.AdjacentTiles())
        {
            SetUnitWhenTesting(tile.transform.position);
            CoupMove move = new CoupMove(tile.transform.position);
            Node node = new Node(move, current.depth + 1);

            Min(node, maxDepth);

            if (current.score < node.score || current.score == Node.initialScore)
            {
                current.score = node.score;
                bestCoup = node.coup;
            }
        }

        SetUnitWhenTesting(IATile.transform.position);
        return bestCoup;
    }

    private Coup Min(Node current, int maxDepth)
    {
        if (current.depth == maxDepth)
        {
            current.score = CalculScore();
            return current.coup;
        }

        BaseUnit player = OtherUnit();
        Coup bestCoup = null;

        if (player.wallCount > 0)
        {
            foreach (KeyValuePair<Vector2, CustomCorner> pair in GridManager.Instance.cornersDico)
            {
                if (HorizontalWall.CanSpawnHere(pair.Value))
                {
                    player.SpawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Horizontal);
                    Node node = new Node(coupWall, current.depth + 1);

                    Max(node, maxDepth);

                    if (node.score < current.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    player.DespawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                }
            
                if (VerticalWall.CanSpawnHere(pair.Value))
                {
                    player.SpawnWallWhenTesting(pair.Key, Orientation.Vertical);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Vertical);
                    Node node = new Node(coupWall, current.depth + 1);

                    Max(node, maxDepth);

                    if (current.score < node.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    player.DespawnWallWhenTesting(pair.Key, Orientation.Vertical);
                }
            }
        }

        CustomTile playerTile = player.occupiedTile;

        foreach (CustomTile tile in playerTile.AdjacentTiles())
        {
            player.SetUnitWhenTesting(tile.transform.position);
            CoupMove move = new CoupMove(tile.transform.position);
            Node node = new Node(move, current.depth + 1);

            Max(node, maxDepth);

            if (current.score > node.score || current.score == Node.initialScore)
            {
                current.score = node.score;
                bestCoup = node.coup;
            }
        }

        player.SetUnitWhenTesting(playerTile.transform.position);
        return bestCoup;
    }
}