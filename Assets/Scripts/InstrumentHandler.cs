using System;
using DG.Tweening;
using UnityEngine;

public class InstrumentHandler : MonoBehaviour, IInteractable
{
    [SerializeField] private Light assignedLight;
    [SerializeField] private int parameterIndex;
    [SerializeField] private bool status;
    public Transform ghostSpawnPoint;
    
    public bool Status => status;
    private float _lightStartingIntensity;

    private void Awake()
    {
        _lightStartingIntensity = assignedLight.intensity;
        
        assignedLight.enabled = status;
        assignedLight.DOIntensity(status ? _lightStartingIntensity : 0f, 1f).SetEase(Ease.InOutSine);
        AudioChannelManager.instance.SetParameterValue(parameterIndex, status ? 0.6f : 0);
        SetInteractableStatus(false);
    }

    public void Punch()
    {
        if (status)
        {
            transform.DOPunchScale(transform.localScale * 0.08f, 0.15f, 40, 3);
        }
    }
    

    public void ToggleStatus()
    {
        status = !status;
        assignedLight.enabled = status;
        assignedLight.DOIntensity(status ? _lightStartingIntensity : 0f, 1f).SetEase(Ease.InOutSine);
        
        AudioChannelManager.instance.SetParameterValue(parameterIndex, status ? 0.6f : 0);
        if (parameterIndex == 3)
        {
            GameManager.isWaitingForBoomboxInteraction = false;
        }
        if (parameterIndex == 0)
        {
            GameManager.isWaitingForBassInteraction = false;
        }
    }

    public void SetInteractableStatus(bool isInteractable)
    {
        gameObject.layer = isInteractable ? 6 : 0;
    }
    
    public void Interact()
    {
        ToggleStatus();
    }
}
