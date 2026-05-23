using System;
using UnityEngine;

public class InteractionDetectionHandler : MonoBehaviour
{
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private Camera mainCam;

    private InstrumentHandler instrument;
    private bool isInteractionPossible;
    
    private void Update()
    {
        var ray = new Ray(transform.position + Vector3.up , transform.forward);
        RaycastHit hit;
        var cast = Physics.SphereCastAll(ray, 0.4f, 0.3f, 64);

        foreach (var element in cast)
        {
            var instrument = element.transform.gameObject.GetComponent<InstrumentHandler>();
            if (instrument != null)
            {
                interactionUI.gameObject.SetActive(true);
                var pos = mainCam.WorldToScreenPoint(instrument.transform.position);
                interactionUI.transform.position = pos;
                this.instrument = instrument;
                isInteractionPossible = true;
            }
        }
        if(cast.Length == 0)
        {
            instrument = null;
            if (isInteractionPossible)
            { 
                interactionUI.gameObject.SetActive(false);
            }
            isInteractionPossible = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && isInteractionPossible)
        {
            instrument.ToggleStatus();
        }
    }
}
