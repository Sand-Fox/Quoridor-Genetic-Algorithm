using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomWall : MonoBehaviour
{
    public abstract Orientation orientation { get; }

    [SerializeField] private CustomCorner corner;
    [SerializeField] private SpriteRenderer wall1;
    [SerializeField] private SpriteRenderer wall2;

    public void EnablePreview(bool enable)
    {
        if (enable)
        {
            gameObject.SetActive(true);
            bool canSpawnHere = (orientation == Orientation.Horizontal) ? HorizontalWall.CanSpawnHere(corner) : VerticalWall.CanSpawnHere(corner);
            if (canSpawnHere)
            {
                wall1.color = ColorExtension.transparent;
                wall2.color = ColorExtension.transparent;
            }
            else
            {
                wall1.color = ColorExtension.transparentRed;
                wall2.color = ColorExtension.transparentRed;
            }
        }

        else gameObject.SetActive(false);
    }

    public void EnableVisual()
    {
        gameObject.SetActive(true);
        wall1.color = Color.white;
        wall2.color = Color.white;
    }

    public abstract void OnSpawn();
    public abstract void OnDespawn();
}
