using System;
using UnityEngine;

public class InteractionDetectionHandler : MonoBehaviour
{
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private Camera mainCam;

    private IInteractable interactable;
    private bool isInteractionPossible;
    
    private void Update()
    {
        if (DialoguePanel.IsDialogueActive)
        {
            interactionUI.gameObject.SetActive(false);
            isInteractionPossible = false;
            return;
        }
        
        var ray = new Ray(transform.position + Vector3.up , transform.forward);
        RaycastHit hit;
        var cast = Physics.SphereCastAll(ray, 0.4f, 0.3f, 64);

        foreach (var element in cast)
        {
            var instrument = element.transform.gameObject.GetComponent<IInteractable>();
            if (instrument != null)
            {
                interactionUI.gameObject.SetActive(true);
                var pos = mainCam.WorldToScreenPoint(instrument.transform.position);
                interactionUI.transform.position = pos;
                interactable = instrument;
                isInteractionPossible = true;
            }
        }
        if(cast.Length == 0)
        {
            interactable = null;
            if (isInteractionPossible)
            { 
                interactionUI.gameObject.SetActive(false);
            }
            isInteractionPossible = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && isInteractionPossible)
        {
            interactable?.Interact();
            GameManager.instance.CheckForCompletion();
        }
    }
}
