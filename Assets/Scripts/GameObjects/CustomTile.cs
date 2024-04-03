using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomTile : MonoBehaviour
{
    [HideInInspector] public BaseUnit occupiedUnit;
    [SerializeField] private GameObject mouseOver;
    [SerializeField] private GameObject visual;
    private bool isTarget = false;

    [Header("Pathfinding Variables")]
    public int G;
    public int H;
    public int F => G + H;
    public CustomTile previousTile;

    public Dictionary<Vector2, bool> directionDico = new();

    private void Awake()
    {
        directionDico.Add(Vector2.right, true);
        directionDico.Add(Vector2.left, true);
        directionDico.Add(Vector2.up, true);
        directionDico.Add(Vector2.down, true);
    }

    private void OnMouseEnter()
    {
        if (!GameManager.Instance.isPlayerTurn()) return;
        if (ModeManager.Instance.mode == Mode.Move) mouseOver.SetActive(true);
    }

    public void OnMouseExit()
    {
        if (!GameManager.Instance.isPlayerTurn()) return;
        if (ModeManager.Instance.mode == Mode.Move) mouseOver.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (!GameManager.Instance.isPlayerTurn()) return;
        if (ModeManager.Instance.mode != Mode.Move || !isTarget) return;
        mouseOver.SetActive(false);
        ReferenceManager.Instance.player.view.RPC("SetUnit", Photon.Pun.RpcTarget.All, transform.position);
    }

    public void EnableVisual(bool enable)
    {
        isTarget = enable;
        visual.SetActive(enable);
    }
}
