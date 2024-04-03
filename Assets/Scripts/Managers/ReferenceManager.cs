using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    public static ReferenceManager Instance;

    private void Awake() => Instance = this;

    [Header("In Game")]
    public BaseUnit player;
    public BaseUnit enemy;

    [Header("Resources Prefabs")]
    public HorizontalWall horizontalWallPrefab;
    public VerticalWall verticalWallPrefab;
}
