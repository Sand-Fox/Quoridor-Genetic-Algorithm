using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelScript : MonoBehaviour
{
    [SerializeField] private Image blackImage;
    [SerializeField] private Button saveButton;
    private RectTransform rectTransform;

    private Tween tweenAnchorPos;
    private Tween tweenFade;

    [SerializeField] private float delay = 0;
    public bool isEnable = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        tweenAnchorPos = rectTransform.DOAnchorPos(Vector2.zero, 0.4f)
            .SetEase(Ease.InOutBack).SetDelay(delay).Pause().SetAutoKill(false);

        tweenFade = blackImage.DOFade(0.3f, 0.4f).From(0).SetEase(Ease.InOutSine).SetDelay(delay)
            .OnRewind(() => blackImage.enabled = false).OnPlay(() => blackImage.enabled = true).Pause().SetAutoKill(false);
    }

    private void OnDestroy()
    {
        rectTransform.DOKill();
    }

    public void EnablePanel(bool enable)
    {
        ModeManager.Instance.UpdateMode(Mode.Normal);
        isEnable = enable;
        if (enable)
        {
            tweenFade.PlayForward();
            tweenAnchorPos.PlayForward();
        }
        else
        {
            tweenFade.PlayBackwards();
            tweenAnchorPos.PlayBackwards();
        }
    }

    public void DisableSaveButton()
    {
        saveButton.interactable = false;
    }

    public void OnImageClick()
    {
        if (GameManager.Instance.gameState == GameState.Win || GameManager.Instance.gameState == GameState.Loose) return;
        EnablePanel(false);
    }
}
