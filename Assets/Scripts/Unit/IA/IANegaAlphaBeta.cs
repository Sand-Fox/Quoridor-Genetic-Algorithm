using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class IANegaAlphaBeta : BaseIA
{
    public static string description = "AI that chooses the best move using\nNega Alpha Beta algorithm";
    public Vector4 weight;
    public int defaultDepth = 2;

    protected override void PlayIA()
    {
        List<CustomTile> pathIA = PathFinding.Instance.GetWiningPath(this);
        List<CustomTile> pathP = PathFinding.Instance.GetWiningPath(OtherUnit());

        // Si le chemin est nul du au deuxieme joueur qui bloque physiquement le passage
        if (pathIA == null)
        {
            Debug.Log("Pas de meilleur chemin trouvé", this);
            SetUnit(occupiedTile.AdjacentTiles()[0].transform.position);
            return;
        }

        // Si on n'a pas de mur on parcourt le plus court chemin
        if (pathP == null || wallCount == 0)
        {
            SetUnit(pathIA[0].transform.position);
            return;
        }

        // Génération du coup, on va maximiser le coup que l'on va jouer donc on appelle Max
        Coup coup = BestCoup(defaultDepth,-10000, 10000, 1);
        
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

        if (pathIA == null || pathP == null) return 0;

        int nbWallIA = wallCount;
        int nbWallP = OtherUnit().wallCount;

        float score = weight.x * pathP.Count * pathP.Count - weight.y * pathIA.Count * pathIA.Count - weight.z * nbWallP + weight.w * nbWallIA;
        //float score = weight.x * pathP.Count - weight.y * pathIA.Count - weight.z * nbWallP + weight.w * nbWallIA;
        return score;
    }

    private float NegaMax(int depth, float alpha, float beta, int maximazingPlayer)
    {
        //Node current : Node a evaluer
        //int maxDepth : Profondeur a laquelle on doit aller
        //int maximazingPlayer : 1 si le joueur veut maximiser, -1 si le joueur veut minimiser

        // Cas de base
        if (depth == 0) return maximazingPlayer * CalculScore();

        // Initialisation du meilleur coup
        float value = -10000;

        // Enfant ou le joueur bouge
        BaseUnit playing = (maximazingPlayer == 1)?this:OtherUnit();
        CustomTile usedTile = playing.occupiedTile;

        foreach(CustomTile tile in usedTile.AdjacentTiles())
        {
            playing.SetUnitWhenTesting(tile.transform.position);
            value = Mathf.Max(value, -NegaMax(depth-1, -beta, -alpha, -maximazingPlayer));

            alpha = Mathf.Max(alpha, value);
            if(alpha >= beta) 
            {
                playing.SetUnitWhenTesting(usedTile.transform.position);
                return value;
            }
        }
        playing.SetUnitWhenTesting(usedTile.transform.position);

        // Si maximazingPlayer = 1, c'est cet Unit qui veut jouer, sinon c'est l'autre unit
        if((maximazingPlayer == 1)?wallCount > 0: OtherUnit().wallCount>0)
        {
            foreach(KeyValuePair < Vector2, CustomCorner > pair in GridManager.Instance.cornersDico)
            {
                // Enfant ou le mur est pose horizontalement
                if(HorizontalWall.CanSpawnHere(pair.Value))
                {
                    SpawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                    value = Mathf.Max(value, -NegaMax(depth-1, -beta, -alpha, -maximazingPlayer));
                    DespawnWallWhenTesting(pair.Key, Orientation.Horizontal);

                    alpha = Mathf.Max(alpha, value);
                    if(alpha >= beta) break;
                }
                // Enfant ou le mur est pose verticalement
                if(VerticalWall.CanSpawnHere(pair.Value))
                {
                    SpawnWallWhenTesting(pair.Key, Orientation.Vertical);
                    value = Mathf.Max(value, -NegaMax(depth-1, -beta, -alpha, -maximazingPlayer));
                    DespawnWallWhenTesting(pair.Key, Orientation.Vertical);

                    alpha = Mathf.Max(alpha, value);
                    if(alpha >= beta) break;
                }
            }
        }

        return value;
    }

    // En réalité même algorithme que plus haut, il renvoit juste le coup de la derniere hauteur au lieu du score
    private Coup BestCoup(int depth, float alpha, float beta, int maximazingPlayer)
    {
        //Node current : Node a evaluer
        //int maxDepth : Profondeur a laquelle on doit aller
        //int maximazingPlayer : 1 si le joueur veut maximiser, -1 si le joueur veut minimiser

        if (depth <= 0) return default;
        
        // Initialisation du meilleur coup
        Coup bestCoup= default;
        float value = -10000;

        BaseUnit playing = (maximazingPlayer == 1)?this:OtherUnit();
        CustomTile usedTile = playing.occupiedTile;

        // Enfant ou le joueur bouge
        foreach(CustomTile tile in usedTile.AdjacentTiles())
        {
            playing.SetUnitWhenTesting(tile.transform.position);
            CoupMove coupMove = new CoupMove(tile.transform.position);
            float score = -NegaMax(depth-1, -beta, -alpha, -maximazingPlayer);
            if(score>value)
            {
                value = score;
                bestCoup = coupMove;
            }
            alpha = Mathf.Max(alpha, value);
        }
        playing.SetUnitWhenTesting(usedTile.transform.position);

        // Si maximazingPlayer = 1, c'est cet Unit qui veut jouer, sinon c'est l'autre unit
        if((maximazingPlayer == 1)?wallCount > 0: OtherUnit().wallCount>0)
        {
            foreach(KeyValuePair < Vector2, CustomCorner > pair in GridManager.Instance.cornersDico)
            {
                // Enfant ou le mur est pose horizontalement
                if(HorizontalWall.CanSpawnHere(pair.Value))
                {
                    SpawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Horizontal);
                    float score = -NegaMax(depth-1, -beta, -alpha, -maximazingPlayer);
                    if(score>value)
                    {
                        value = score;
                        bestCoup = coupWall; 
                    }
                    DespawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                    alpha = Mathf.Max(alpha, value);
                }
                // Enfant ou le mur est pose verticalement
                if(VerticalWall.CanSpawnHere(pair.Value))
                {
                    SpawnWallWhenTesting(pair.Key, Orientation.Vertical);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Vertical);
                    float score = -NegaMax(depth-1, -beta, -alpha, -maximazingPlayer);
                    if(score>value)
                    {
                        value = score;
                        bestCoup = coupWall; 
                    }
                    DespawnWallWhenTesting(pair.Key, Orientation.Vertical);
                    alpha = Mathf.Max(alpha, value);
                }
            }
        }

        return bestCoup;
    }
}