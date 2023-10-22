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

    [SerializeField] Dialogue[] phaseDialogue;
    [SerializeField] string[] phaseRequiredItem;
    private int phaseIndex;
    private int maxPhaseIndex;

    private bool firstDialogueTriggered;
    private bool canStartPhaseDialogue;

    private void Awake()
    {
        TryGetComponent(out keymasterScript);

        dialogueIndex = 0;
        phaseIndex = 0;
        maxIndex = dialogue.Length - 1;
        maxPhaseIndex = phaseDialogue.Length - 1;
        firstDialogueTriggered = false;
        canStartPhaseDialogue = false;
    }

    private IEnumerator Start()
    {
        keymasterScript.SetCanSelect(false);

        yield return new WaitForSeconds(2);
        //DialogueManager.Instance.StartDialogue(dialogue[dialogueIndex], true);

        DialogueManager.Instance.AddDialogueQueue(new DialoguePlayer(dialogue[dialogueIndex], true));
    }

    private void Update()
    {
        //PlayPhaseDialogue();

        if (keymasterScript.SelectedItemName == phaseRequiredItem[phaseIndex])
        {
            NextPhase();
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
        StartCoroutine(StartPhaseDialogue());
        keymasterScript.SetCanSelect(true);
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
        
        StartCoroutine(StartPhaseDialogue());
    }

    IEnumerator StartPhaseDialogue()
    {
        canStartPhaseDialogue = false;

        DialogueManager.Instance.StartDialogue(phaseDialogue[phaseIndex], false);
        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);

        canStartPhaseDialogue = true;
    }

    void NextPhase()
    {
        phaseIndex++;
        if (phaseIndex > maxPhaseIndex)
        {
            phaseIndex = maxPhaseIndex;
        }
    }
}
