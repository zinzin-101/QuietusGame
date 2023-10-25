using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KeyMasterDetect))]
public class PallorMortisScript : MonoBehaviour
{
    private KeyMasterDetect keymasterScript;

    [SerializeField] Dialogue[] dialogue;
    private int dialogueIndex;
    private int maxIndex;

    [SerializeField] Dialogue[] itemDialogue;
    [SerializeField] Dialogue[] phaseDialogue;
    [SerializeField] Dialogue[] phaseDialogueAfter;
    [SerializeField] Item[] phaseRequiredItem = new Item[3];
    private int phaseIndex;
    private int maxPhaseIndex;

    private bool firstDialogueTriggered;
    private bool canStartPhaseDialogue;

    private bool[] firstPhaseDialoguePlayed = new bool[3];


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

        for (int i = 0; i < firstPhaseDialoguePlayed.Length; i++)
        {
            firstPhaseDialoguePlayed[i] = false;
        }
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
            StartCoroutine(NextPhase());
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
        //StartCoroutine(StartPhaseDialogue());
        PlayPhaseDialogue();
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

        switch (firstPhaseDialoguePlayed[phaseIndex])
        {
            case true:
                StartCoroutine(StartPhaseDialogue());
                break;
            case false:
                StartCoroutine(StartFirstPhaseDialogue());
                break;
        }
    }

    IEnumerator StartPhaseDialogue()
    {
        canStartPhaseDialogue = false;

        DialogueManager.Instance.StartDialogue(phaseDialogueAfter[phaseIndex], true);
        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);

        canStartPhaseDialogue = true;
    }

    IEnumerator StartFirstPhaseDialogue()
    {
        firstPhaseDialoguePlayed[phaseIndex] = true;
        canStartPhaseDialogue = false;

        DialogueManager.Instance.StartDialogue(phaseDialogue[phaseIndex], false);
        
        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);

        canStartPhaseDialogue = true;
    }

    IEnumerator NextPhase()
    {
        DialogueManager.Instance.ResetDialogue();

        GameManager.Instance.AllowPlayerToSit(false);

        DialogueManager.Instance.StartDialogue(itemDialogue[phaseIndex], true);

        phaseIndex++;
        if (phaseIndex > maxPhaseIndex)
        {
            phaseIndex = maxPhaseIndex;
        }

        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);

        GameManager.Instance.AllowPlayerToSit(true);
    }
}
