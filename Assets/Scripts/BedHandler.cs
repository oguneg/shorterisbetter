using UnityEngine;

public class BedHandler : MonoBehaviour, IInteractable
{
    private new void Awake()
    {
        
    }
    
    public void ToggleStatus()
    {
        GameManager.isWaitingForBedInteraction = false;
    }

    public void Interact()
    {
        ToggleStatus();
    }
}
