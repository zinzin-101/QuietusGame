using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KeyMasterDetect))]
public class PallorMortisScript : MonoBehaviour
{
    public enum Phases
    {
        Before,
        During,
        After
    }

    private KeyMasterDetect keymasterScript;

    [SerializeField] Dialogue[] dialogue;
    private int dialogueIndex;
    private int maxIndex;

    [SerializeField] Dialogue[] phaseDialogueBefore;
    [SerializeField] Dialogue[] phaseDialogue;
    [SerializeField] Dialogue[] phaseDialogueAfter;
    [SerializeField] Item[] phaseRequiredItem = new Item[3];
    private int phaseIndex;
    private int maxPhaseIndex;

    private bool firstDialogueTriggered;
    private bool canStartPhaseDialogue;

    private Phases phaseStatus;

    private void Awake()
    {
        TryGetComponent(out keymasterScript);

        dialogueIndex = 0;
        phaseIndex = 0;
        maxIndex = dialogue.Length - 1;
        maxPhaseIndex = phaseDialogue.Length - 1;
        firstDialogueTriggered = false;
        canStartPhaseDialogue = false;

        for (int i = 0; i < phaseRequiredItem.Length; i++)
        {
            keymasterScript.SetRequiredItem(phaseRequiredItem[i], i);
        }

        phaseStatus = Phases.Before;
    }

    private IEnumerator Start()
    {
        keymasterScript.SetCanInteract(false);

        yield return new WaitForSeconds(2);
        //DialogueManager.Instance.StartDialogue(dialogue[dialogueIndex], true);

        DialogueManager.Instance.AddDialogueQueue(new DialoguePlayer(dialogue[dialogueIndex], true));
    }

    private void Update()
    {
        //PlayPhaseDialogue();

        if (keymasterScript.HasItem)
        {
            NextPhase();
            keymasterScript.SetHasItem(false);
        }
    }

    void TriggerNextDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex <= maxIndex)
        {
            //DialogueManager.Instance.StartDialogue(dialogue[dialogueIndex], true);
            DialogueManager.Instance.AddDialogueQueue(new DialoguePlayer(dialogue[dialogueIndex], true));
        }
    }

    IEnumerator FirstDialogue()
    {
        GameManager.Instance.AllowPlayerToSit(false);
        TriggerNextDialogue();
        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);
        TriggerNextDialogue();
        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);
        firstDialogueTriggered = true;
        canStartPhaseDialogue = true;
        GameManager.Instance.AllowPlayerToSit(true);
        yield return new WaitForSeconds(3.5f);
        StartCoroutine(StartPhaseDialogue(phaseStatus));
        keymasterScript.SetCanInteract(true);
    }

    public void TriggerFirstDialogue()
    {
        if (firstDialogueTriggered) return;

        StartCoroutine(FirstDialogue());
    }

    public void PlayPhaseDialogue()
    {
        if (GameManager.Instance.CurrentRoom != 1 && GameManager.Instance.CanStartDialogue) return;
        if (!canStartPhaseDialogue) return;

        if (phaseStatus == Phases.Before)
        {
            if (phaseIndex == 0)
            {
                phaseStatus = Phases.After;
                return;
            }
            StartCoroutine(StartPhaseDialogue(phaseStatus));
            phaseStatus = Phases.During;
            return;
        }

        if (phaseStatus == Phases.During)
        {
            StartCoroutine(StartPhaseDialogue(phaseStatus));
            phaseStatus = Phases.After;
            return;
        }
        
        StartCoroutine(StartPhaseDialogue(phaseStatus));
    }



    IEnumerator StartPhaseDialogue(Phases phase)
    {
        canStartPhaseDialogue = false;

        switch (phase)
        {
            case Phases.Before:
                if (phaseIndex == 0)
                {
                    phaseStatus = Phases.During;
                    break;
                }
                DialogueManager.Instance.StartDialogue(phaseDialogueBefore[phaseIndex], true);
                break;

            case Phases.During:
                DialogueManager.Instance.StartDialogue(phaseDialogue[phaseIndex], false);
                break;

            case Phases.After:
                DialogueManager.Instance.StartDialogue(phaseDialogueAfter[phaseIndex], true);
                break;
        }
        if (phaseStatus == Phases.During && phaseIndex == 0)
        {
            phaseStatus = Phases.After;
        }

        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);

        canStartPhaseDialogue = true;
    }

    void NextPhase()
    {
        DialogueManager.Instance.ResetDialogue();

        phaseIndex++;
        if (phaseIndex > maxPhaseIndex)
        {
            phaseIndex = maxPhaseIndex;
        }

        phaseStatus = Phases.Before;

        if (phaseIndex == maxPhaseIndex - 1)
        {
            PlayPhaseDialogue();
            phaseIndex++;
            return;
        }

        PlayPhaseDialogue();
    }
}
