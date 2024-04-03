using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class ScrollbarScript : MonoBehaviour
{
    [SerializeField] private PartieButton partiePanelPrefab;

    private void Start()
    {
        foreach(FileInfo file in SaveSystem.GetFiles())
        {
            PartieButton partiePanel = Instantiate(partiePanelPrefab, transform);
            partiePanel.SetUp(file);
        }
    }
}
