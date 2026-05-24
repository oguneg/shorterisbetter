using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    [SerializeField] private GhostHandler ghostPrefab;

    [SerializeField] private CanvasGroup endingScreen;
    [SerializeField] private TextMeshProUGUI endingText1, endingText2;
    private Coroutine ghostRoutine;
    private bool isGameStarted = false;
    private float punchInterval = 1.08333333f;
    private float lastPunchTime = 0f;
    
    private void Start()
    {
        isInputEnabled = true;
        StartCoroutine(GameRoutine());
        //ghostRoutine = StartCoroutine(GhostRoutine());
    }

    private void Update()
    {
        if (Time.time > lastPunchTime + punchInterval)
        {
            lastPunchTime = Time.time;
            foreach (var element in instruments)
            {
                element.Punch();
            }
        }
    }

    private InstrumentHandler GetDisabledInstrument()
    {
        List<InstrumentHandler> disabledInstruments = new List<InstrumentHandler>();
        foreach (var instrument in instruments)
        {
            if (instrument.Status == false)
            {
                disabledInstruments.Add(instrument);
            }
        }

        if (disabledInstruments.Count == 0)
        {
            return null;
        }
        else
        {
            return disabledInstruments[Random.Range(0, disabledInstruments.Count)];
        }
    }

    IEnumerator GameRoutine()
    {
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
        dialogueManager.TriggerDialogueBatch(4);
        yield return null;
        
        ghostRoutine = StartCoroutine(GhostRoutine());
    }

    IEnumerator GhostRoutine()
    {
        isGameStarted = true;
        SetAllInstrumentsInteractable();
        float spawnDelay = 0.75f;
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay + Random.Range(0.3f, 1f));
            spawnDelay += 0.15f;
            var instrument = GetDisabledInstrument();
            if (instrument == null) continue;
            var ghost = Instantiate(ghostPrefab, transform);
            ghost.Appear(instrument.ghostSpawnPoint, instrument);
        }
    }

    private void ShowEndingScreen()
    {
        endingScreen.DOFade(1, 0.5f);
        endingText1.DOFade(1, 0.5f).SetDelay(2f);
        endingText2.DOFade(1, 0.5f).SetDelay(4f);
        endingScreen.DOFade(0, 0.5f).SetDelay(8f);
    }

    public void CheckForCompletion()
    {
        if (!isGameStarted) return;
        bool result = false;
        foreach (var element in instruments)
        {
            if (element.Status) return;
        }
        StopCoroutine(ghostRoutine);
        var ghosts = FindObjectsOfType<GhostHandler>();
        foreach (var ghost in ghosts)
        {
            DOTween.Kill(ghost);
            Destroy(ghost.gameObject);
        }
        CompleteGame();
    }

    private void CompleteGame()
    {
        isGameStarted = false;
        StartCoroutine(EndingRoutine());
    }

    private IEnumerator EndingRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        dialogueManager.TriggerDialogueBatch(5);
        yield return new WaitWhile(()=>DialoguePanel.IsDialogueActive);
        yield return new WaitForSeconds(0.5f);
        TurnOnAllInstruments();
        ShowEndingScreen();        
    }

    private void TurnOnAllInstruments()
    {
        foreach (var element in instruments)
        {
            if(!element.Status) element.Interact();
        }
    }

    private void SetAllInstrumentsInteractable()
    {
        foreach (var element in instruments)
        {
            element.SetInteractableStatus(true);
        }
    }
    
    
    public void SetInputStatus(bool isEnabled)
    {
        isInputEnabled = isEnabled;
    }
}
