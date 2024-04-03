using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!GameManager.Instance.isPlayerTurn() || ModeManager.Instance.mode != Mode.Wall) return;
        if (Input.GetMouseButtonDown(1))
        {
            CustomCorner corner = GridManager.Instance.selectedCorner;
            corner?.OnMouseExit();
            CustomCorner.SwitchOrientation();
            corner?.OnMouseEnter();
        }
    }
}
