using System;
using System.Collections;
using UnityEngine;

public class DialogueManager : MonoSingleton<DialogueManager>
{
    [SerializeField] private DialogueBatch[] dialogueBatches;
    [SerializeField] private DialoguePanel dialoguePanel;
    private bool _isWaitingForInput = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _isWaitingForInput = false;
        }
    }

    public void TriggerDialogueBatch(int index)
    {
        StartCoroutine(DialogueRoutine(dialogueBatches[index]));
    }
    
    private IEnumerator DialogueRoutine(DialogueBatch batch)
    {
        dialoguePanel.ShowPanel();
        yield return new WaitForSeconds(0.2f);
        var dialogue = batch;
        WaitWhile waitForInput = new WaitWhile(() => _isWaitingForInput);
        int i = 0;
        while (true)
        {
            if (dialoguePanel.IsAnimatingText)
            {
                dialoguePanel.RushMessage(batch.messages[i-1]);
            }
            else
            {
                if (i >= batch.messages.Length) break;
                dialoguePanel.ShowMessage(batch.messages[i]);
                i++;
            }
            _isWaitingForInput = true;
            yield return waitForInput;
        }
        dialoguePanel.HidePanel();
    }
}

[Serializable]
public struct DialogueBatch
{
    public string[] messages;
}