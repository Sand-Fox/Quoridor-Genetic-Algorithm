using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BannerScript : MonoBehaviour
{
    private TextMeshProUGUI TMPtext;
    private RectTransform rectTransform;

    private void Awake()
    {
        TMPtext = transform.GetComponentInChildren<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        rectTransform.DOLocalMoveY(8, 1.25f).SetRelative().SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void ChangeText(string newText)
    {
        if (TMPtext == null)
        {
            Debug.Log("MainText du bouton " + name + " non trouvé");
            return;
        }
        TMPtext.text = newText;
    }

    private void OnDestroy() => rectTransform.DOKill();
}
