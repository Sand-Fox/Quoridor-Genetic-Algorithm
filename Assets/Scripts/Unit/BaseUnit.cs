using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public abstract class BaseUnit : MonoBehaviour
{
    public CustomTile occupiedTile { get; private set; }
    public PhotonView view { get; private set; }
    public int wallCount = 10;

    public static float movementDuration = 0.3f;

    protected virtual void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        CustomTile tile = GridManager.Instance.GetTileAtPosition(new Vector2(4, 0));
        if (this == ReferenceManager.Instance.enemy) tile = GridManager.Instance.GetTileAtPosition(new Vector2(4, 8));
        tile.occupiedUnit = this;
        occupiedTile = tile;
        transform.position = tile.transform.position;
    }

    public BaseUnit OtherUnit()
    {
        if (ReferenceManager.Instance.player == this) return ReferenceManager.Instance.enemy;
        return ReferenceManager.Instance.player;
    }

    [PunRPC]
    public void SetUnit(Vector3 position)
    {
        CustomTile tile = GridManager.Instance.GetTileAtPosition(position);
        if (!view.IsMine) tile = GridManager.Instance.GetTileAtPosition(position.ReflectPosition());

        if (tile.occupiedUnit != null) Debug.LogWarning("Attention, il y a déjà un Unit sur cette case.");

        occupiedTile.occupiedUnit = null;
        tile.occupiedUnit = this;
        occupiedTile = tile;
        transform.DOMove(tile.transform.position, movementDuration).SetEase(Ease.InOutSine);
        AudioManager.Instance.Play("Move");

        CoupMove c = new CoupMove(tile.transform.position);
        RegisterManager.Instance.AddCoup(c);
        GameManager.Instance.EndTurn();
    }

    public void SetUnitWhenTesting(Vector3 position)
    {
        CustomTile tile = GridManager.Instance.GetTileAtPosition(position);

        if (tile.occupiedUnit != null) Debug.LogWarning("Attention, il y a déjà un Unit sur cette case.");

        occupiedTile.occupiedUnit = null;
        tile.occupiedUnit = this;
        occupiedTile = tile;
        transform.position = tile.transform.position;
    }

    [PunRPC]
    public void SpawnWall(Vector3 position, Orientation orientation)
    {
        CustomCorner corner = GridManager.Instance.GetCornerAtPosition(position);
        if (!view.IsMine) corner = GridManager.Instance.GetCornerAtPosition(position.ReflectPosition());

        if (!corner.isOpen) Debug.LogWarning("Attention, il y a déjà un mur sur ce coin.");

        CustomWall wall = (orientation == Orientation.Horizontal) ? corner.horizontalWall : corner.verticalWall;
        wall.EnableVisual();
        wall.OnSpawn();
        wallCount--;
        UIManager.Instance.UpdateWallCountText();
        AudioManager.Instance.Play("Wall");

        CoupWall c = new CoupWall(corner.transform.position, orientation);
        RegisterManager.Instance.AddCoup(c);
        GameManager.Instance.EndTurn();
    }

    public void SpawnWallWhenTesting(Vector3 position, Orientation orientation)
    {
        CustomCorner corner = GridManager.Instance.GetCornerAtPosition(position);

        if (!corner.isOpen) Debug.LogWarning("Attention, il y a déjà un mur sur ce coin.");

        CustomWall wall = (orientation == Orientation.Horizontal) ? corner.horizontalWall : corner.verticalWall;
        wall.OnSpawn();
        wallCount--;
    }

    public void DespawnWallWhenTesting(Vector3 position, Orientation orientation)
    {
        CustomCorner corner = GridManager.Instance.GetCornerAtPosition(position);

        if (corner.isOpen) Debug.LogWarning("Attention, il n'y a pas de mur actif sur ce coin.");

        CustomWall wall = (orientation == Orientation.Horizontal) ? corner.horizontalWall : corner.verticalWall;
        wall.OnDespawn();
        wallCount++;
    }
}
