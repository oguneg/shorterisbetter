using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class GhostHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem cloudParticle;
    [SerializeField] private Transform ghostTransform;
    [SerializeField] private Light ghostLight1, ghostLight2;
    public void Appear(Transform location, IInteractable interactable)
    {
        cloudParticle.Play();
        transform.localEulerAngles = location.localEulerAngles;
        transform.position = location.position;
        
        var seq = DOTween.Sequence();
        seq.SetTarget(this);
        seq.Append(ghostTransform.DOScale(100f, 0.25f).SetEase(Ease.OutBack));
        seq.Append(transform.DOMove(location.position + location.forward * 2f, 1f).SetEase(Ease.InOutSine));
        seq.AppendInterval(0.3f);
        seq.AppendCallback(interactable.Interact);
        seq.AppendInterval(0.2f);
        seq.AppendCallback(cloudParticle.Play);
        seq.Append(ghostTransform.DOScale(0f, 0.2f).SetEase(Ease.InBack));
        seq.AppendInterval(0.5f);
        seq.Append(ghostLight1.DOIntensity(0, 0.3f));
        seq.Join(ghostLight2.DOIntensity(0, 0.3f));
        seq.AppendCallback(()=> gameObject.SetActive(false));
    }
}
