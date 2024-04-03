using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CustomCorner : MonoBehaviour
{
    public static Orientation orientation = Orientation.Horizontal;
    [SerializeField] private GameObject visual;

    public bool isOpen = true;
    public CustomWall horizontalWall;
    public CustomWall verticalWall;

    public void OnMouseEnter()
    {
        if (!GameManager.Instance.isPlayerTurn() || ModeManager.Instance.mode != Mode.Wall || !isOpen) return;
        CustomWall wallPreview = (orientation == Orientation.Horizontal) ? horizontalWall : verticalWall;
        wallPreview.EnablePreview(true);
        GridManager.Instance.selectedCorner = this;
    }

    public void OnMouseExit()
    {
        if (!GameManager.Instance.isPlayerTurn() || ModeManager.Instance.mode != Mode.Wall || !isOpen) return;
        CustomWall wallPreview = (orientation == Orientation.Horizontal) ? horizontalWall : verticalWall;
        wallPreview.EnablePreview(false);
        GridManager.Instance.selectedCorner = null;
    }

    private void OnMouseDown()
    {
        if (!GameManager.Instance.isPlayerTurn() || ModeManager.Instance.mode != Mode.Wall || !isOpen) return;
        bool canSpawnHere = (orientation == Orientation.Horizontal) ? HorizontalWall.CanSpawnHere(this) : VerticalWall.CanSpawnHere(this);
        if (!canSpawnHere) return;
        ReferenceManager.Instance.player.view.RPC("SpawnWall", RpcTarget.All, transform.position, orientation);
        GridManager.Instance.selectedCorner = null;
    }

    public void EnableVisual(bool enable)
    {
        visual.SetActive(enable && isOpen);
    }

    public static void SwitchOrientation()
    {
        orientation = (orientation == Orientation.Horizontal) ? Orientation.Vertical : Orientation.Horizontal;
    }
}

public enum Orientation
{
    Horizontal,
    Vertical
}
