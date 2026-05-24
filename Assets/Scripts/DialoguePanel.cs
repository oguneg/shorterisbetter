using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour
{
    public static bool IsDialogueActive;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private Image panelBG;
    public bool IsAnimatingText { get; private set; } = false;

    public void ShowPanel()
    {
        panelBG.transform.localScale = Vector3.zero;
        textField.text = String.Empty;
        panelBG.gameObject.SetActive(true);
        panelBG.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        IsDialogueActive = true;
    }

    public void HidePanel()
    {
        IsDialogueActive = false;
        
        panelBG.transform.DOScale(0, 0.3f).SetEase(Ease.InBack)
            .OnComplete(() => panelBG.gameObject.SetActive(false));
    }

    public void ShowMessage(string message)
    {
        string animatedStr=String.Empty;
        int animatedStrLength = 0;
        IsAnimatingText = true;
        var typewriterTween = DOTween.To(() => animatedStr, x => animatedStr = x, message, 18).SetSpeedBased().SetEase(Ease.Linear).SetTarget(textField)
            .OnUpdate(() =>
            {
                if (animatedStr.Length > animatedStrLength)
                {
                    animatedStrLength = animatedStr.Length;
                    OnLetter();
                }
                textField.text = animatedStr;
            })
            .OnComplete(() => IsAnimatingText = false);
    }

    public void RushMessage(string message)
    {
        textField.DOKill();
        textField.text = message;
        IsAnimatingText = false;
    }

    private void OnLetter()
    {
        textField.transform.DOShakePosition(0.01f,1,25).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);
    }
}
