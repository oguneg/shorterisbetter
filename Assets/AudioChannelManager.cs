using DG.Tweening;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioChannelManager : MonoSingleton<AudioChannelManager>
{
    [Header("FMOD Settings")]
    [SerializeField] private StudioEventEmitter eventEmitter;
    [SerializeField] private string parameter1Name = "Intensity";
    [SerializeField] private string parameter2Name = "Drums";
    [SerializeField] private string parameter3Name = "Drums";
    [SerializeField] private string parameter4Name = "Drums";
    [SerializeField] private string parameter5Name = "Drums";

    [SerializeField][Range(0,1)] private float[] parameterValues;
    
    private EventInstance eventInstance;
    void Start()
    {
        if (eventEmitter != null)
        {
            eventInstance = eventEmitter.EventInstance;
        }
        
    }

    public void SetParameterValue(int parameterIndex, float val)
    {
        DOTween.To(()=>parameterValues[parameterIndex], x => parameterValues[parameterIndex] = x, val, 1).SetSpeedBased().SetEase(Ease.InOutSine);
        
        //parameterValues[parameterIndex] = val;
    }
    
    void Update()
    {
        UpdateIntensityParameter();
    }
    
    void UpdateIntensityParameter()
    {
        if (!eventInstance.isValid())
            return;
        
        // Set the FMOD parameter
        eventInstance.setParameterByName(parameter1Name, parameterValues[0]);
        eventInstance.setParameterByName(parameter2Name, parameterValues[1]);
        eventInstance.setParameterByName(parameter3Name, parameterValues[2]);
        eventInstance.setParameterByName(parameter4Name, parameterValues[3]);
        eventInstance.setParameterByName(parameter5Name, parameterValues[4]);
    }
    
    void OnDestroy()
    {
        // Clean up the event instance
        if (eventInstance.isValid())
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventInstance.release();
        }
    }
}