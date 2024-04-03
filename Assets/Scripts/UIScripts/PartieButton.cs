using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class PartieButton : MonoBehaviour
{
    private FileInfo file;

    public void SetUp(FileInfo _file)
    {
        file = _file;
        GetComponentInChildren<TextMeshProUGUI>().text = _file.Name;
    }

    public void OnPlayButton()
    {
        Partie partie = SaveSystem.Load<Partie>(file.Name);
        SceneSetUpManager.replay = partie;
        PrivateRoom.Instance.CreatePrivateRoom();
    }

    public void OnDeleteButton()
    {
        file.Delete();
        Destroy(gameObject);
    }
}
