using UnityEngine;

public class BedHandler : MonoBehaviour, IInteractable
{
    private new void Awake()
    {
        
    }
    
    public void ToggleStatus()
    {
        GameManager.isWaitingForBedInteraction = false;
        gameObject.layer = 0;

    }

    public void Interact()
    {
        ToggleStatus();
    }
}
