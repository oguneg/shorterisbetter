using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class GhostHandler : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [FormerlySerializedAs("endposition")] [SerializeField] private Transform endPosition;
    private void Start()
    {
        //transform.DOLocalMoveY(0.1f, 1f).SetRelative().SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        transform.position = startPosition.position;
        transform.LookAt(endPosition.position);
        
        var seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.Append(transform.DOMove(endPosition.position, 2f));
        seq.Join(transform.DOLookAt(endPosition.position, 0.3f));
        seq.AppendInterval(0.5f);
        seq.Append(transform.DOMove(startPosition.position, 2f));
        seq.Join(transform.DOLookAt(startPosition.position, 0.3f));
        seq.SetLoops(-1, LoopType.Restart);
        seq.Play();
    }
}
