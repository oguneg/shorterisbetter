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
    
    [SerializeField][Range(0,1)] private float parameter1Value; 
    [SerializeField][Range(0,1)] private float parameter2Value; 
    [SerializeField][Range(0,1)] private float parameter3Value; 
    [SerializeField][Range(0,1)] private float parameter4Value; 
    [SerializeField][Range(0,1)] private float parameter5Value; 
    
    private EventInstance eventInstance;
    
    void Start()
    {
        if (eventEmitter != null)
        {
            eventInstance = eventEmitter.EventInstance;
        }
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
        eventInstance.setParameterByName(parameter1Name, parameter1Value);
        eventInstance.setParameterByName(parameter2Name, parameter2Value);
        eventInstance.setParameterByName(parameter3Name, parameter3Value);
        eventInstance.setParameterByName(parameter4Name, parameter4Value);
        eventInstance.setParameterByName(parameter5Name, parameter5Value);
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