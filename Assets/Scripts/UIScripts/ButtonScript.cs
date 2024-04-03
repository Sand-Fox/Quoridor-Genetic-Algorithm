using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ButtonScript : MonoBehaviour
{
    private RectTransform rectTransform;
    private Button button;
    private TextMeshProUGUI TmpText;

    Tween tweenScale;
    Tween tweenShake;

    [SerializeField] private float scale = 1.1f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        TmpText = GetComponentInChildren<TextMeshProUGUI>();

        tweenScale = rectTransform.DOScale(scale, 0.15f).SetEase(Ease.InOutSine).Pause().SetAutoKill(false);
        tweenShake = rectTransform.DOShakeAnchorPos(0.4f, 5).Pause().SetAutoKill(false);
    }

    private void OnDestroy()
    {
        rectTransform.DOKill();
    }

    public void OnPointerEnter()
    {
        tweenScale.PlayForward();
    }

    public void OnPointerExit()
    {
        tweenScale.PlayBackwards();
    }

    public void OnPointerClick()
    {
        tweenShake.Restart();
        AudioManager.Instance.Play("Click");
    }

    public void EnableButton(bool enable)
    {
        button.interactable = enable;
    }

    public void ChangeMainText(string newText)
    {
        if (TmpText == null)
        {
            Debug.Log("MainText du bouton " + name + " non trouvé");
            return;
        }
        TmpText.text = newText;
    }
}
