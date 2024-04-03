using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePopUp : MonoBehaviour
{
    public static VoidEventChannelSO destroyPopUpsEventChannel => Resources.Load<VoidEventChannelSO>("DestroyPopUpsEventChannel");
    [SerializeField] private float arrowCountPerTile;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject targetPrefab;

    private void OnEnable() => destroyPopUpsEventChannel.OnVoidEvent += DestroyPopUp;
    private void OnDisable() => destroyPopUpsEventChannel.OnVoidEvent -= DestroyPopUp;

    private void DestroyPopUp()
    {
        destroyPopUpsEventChannel.OnVoidEvent -= DestroyPopUp;
        Destroy(gameObject);
    }

    public static LinePopUp Create(List<CustomTile> tiles, Color color)
    {
        GameObject popUpPrefab = Resources.Load<GameObject>("LinePopUp");
        GameObject linePopUpObject = Instantiate(popUpPrefab, Vector3.zero, Quaternion.identity);
        LinePopUp linePopUp = linePopUpObject.GetComponent<LinePopUp>();
        linePopUp.SetUp(tiles, color);
        return linePopUp;
    }

    private void SetUp(List<CustomTile> tiles, Color color)
    {
        if (tiles.Count < 2) return;
        List<Line> lines = GetLines(tiles);

        foreach (Line line in lines)
        {
            GameObject point = Instantiate(pointPrefab, line.point1, Quaternion.identity, transform);
            point.GetComponent<SpriteRenderer>().color = color;

            int arrowCount = (int)(arrowCountPerTile * line.length);
            float distanceBetweenArrows = line.length / (arrowCount + 1);

            for (int count = 1; count <= arrowCount; count++)
            {
                GameObject arrow = Instantiate(arrowPrefab, line.point1 + count * distanceBetweenArrows * line.directionNormalize, Quaternion.identity, transform);
                arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, line.direction)));
                arrow.GetComponent<SpriteRenderer>().color = color;
            }
        }

        GameObject circle = Instantiate(targetPrefab, lines[lines.Count - 1].point2, Quaternion.identity, transform);
        circle.GetComponent<SpriteRenderer>().color = color;
    }

    private List<Line> GetLines(List<CustomTile> tiles)
    {
        List<Line> lines = new List<Line>();
        CustomTile lastTile = tiles[0];

        for (int i = 1; i < tiles.Count - 1; i++)
        {
            Vector3 directionBefore = tiles[i - 1].DirectionTo(tiles[i]);
            Vector3 directionAfter = tiles[i].DirectionTo(tiles[i + 1]);

            if (directionBefore != directionAfter)
            {
                Line line = new Line(lastTile, tiles[i]);
                lines.Add(line);
                lastTile = tiles[i];
            }
        }

        Line finalLine = new Line(lastTile, tiles[tiles.Count - 1]);
        lines.Add(finalLine);
        return lines;
    }

    private struct Line
    {
        public Vector2 point1 { get; private set; }
        public Vector2 point2 { get; private set; }

        public Vector2 direction => point2 - point1;
        public Vector2 directionNormalize => direction.normalized;
        public float length => direction.magnitude;

        public Line(CustomTile tile1, CustomTile tile2)
        {
            point1 = tile1.transform.position;
            point2 = tile2.transform.position;
        }
    }
}
