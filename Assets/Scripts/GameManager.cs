using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public static bool isWaitingForBedInteraction;
    public static bool isWaitingForBoomboxInteraction;
    public static bool isWaitingForBassInteraction;

    public static bool isInputEnabled;
    
    [SerializeField] private Light mainLight;
    [SerializeField] private List<InstrumentHandler> instruments;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Image blackoutImage;
    private void Start()
    {
        StartCoroutine(GameRoutine());
    }


    IEnumerator GameRoutine()
    {
        isInputEnabled = true;
        yield return new WaitForSeconds(1);
        dialogueManager.TriggerDialogueBatch(0);
        isWaitingForBedInteraction = true;
        yield return new WaitUntil(() => isWaitingForBedInteraction == false);
        //sleepy sleep routine
        isInputEnabled = false;
        mainLight.DOIntensity(0, 0.2f);
        blackoutImage.DOFade(1, 0.2f).SetDelay(1f);
        yield return new WaitForSeconds(3f);
        
        instruments[0].Interact();
        instruments[0].SetInteractableStatus(true);
        yield return new WaitForSeconds(1f);
        dialogueManager.TriggerDialogueBatch(1);
        isInputEnabled = true;
        
        blackoutImage.DOFade(0, 0.2f).SetDelay(2);

        isWaitingForBoomboxInteraction = true;
        yield return new WaitUntil(() => isWaitingForBoomboxInteraction == false);
        instruments[0].SetInteractableStatus(false);
        
        dialogueManager.TriggerDialogueBatch(2);
        yield return new WaitWhile(() => DialoguePanel.IsDialogueActive);
        yield return new WaitForSeconds(0.6f);
        
        instruments[1].Interact();
        instruments[1].SetInteractableStatus(true);

        
        yield return new WaitForSeconds(0.3f);
        
        dialogueManager.TriggerDialogueBatch(3);
        isWaitingForBassInteraction = true;
        yield return new WaitUntil(() => isWaitingForBassInteraction == false);
        instruments[0].SetInteractableStatus(true);
        instruments[2].SetInteractableStatus(true);
        instruments[3].SetInteractableStatus(true);
        instruments[4].SetInteractableStatus(true);
        
        yield return null;
    }

    public void SetInputStatus(bool isEnabled)
    {
        isInputEnabled = isEnabled;
    }
}
