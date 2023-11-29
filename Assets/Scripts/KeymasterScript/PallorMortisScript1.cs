using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KeyMasterDetect))]
public class PallorMortisScript1 : MonoBehaviour
{
    private KeyMasterDetect keymasterScript;

    [SerializeField] Dialogue[] dialogue;
    private int dialogueIndex;
    private int maxIndex;

    [SerializeField] Dialogue[] loop;
    private int loopIndex;
    private int maxLoopIndex;

    [SerializeField] Dialogue[] box;
    private int boxIndex;
    private int maxBoxIndex;

    private bool firstDialogueTriggered;

    [SerializeField] PallorAnimation pallorAnim;

    private void Awake()
    {
        TryGetComponent(out keymasterScript);

        dialogueIndex = 0;
        maxIndex = dialogue.Length - 1;

        loopIndex = 0;
        maxLoopIndex = loop.Length - 1;

        boxIndex = 0;
        maxBoxIndex = loop.Length - 1;

        firstDialogueTriggered = false;
    }

    private IEnumerator Start()
    {
        keymasterScript.SetCanInteract(false);
        GameManager.Instance.TimerForcedStop(true);
        //GameManager.Instance.AllowPlayerToMove(false);
        yield return new WaitForSeconds(1f);
        DialogueManager.Instance.StartDialogue(dialogue[dialogueIndex], true);
    }

    private void Update()
    {
        //PlayPhaseDialogue();

        //if (keymasterScript.HasItem)
        //{
        //    keymasterScript.SetHasItem(false);
        //    StartCoroutine(NextPhase());
        //}
    }

    public void TriggerFirstDialogue()
    {
        if (firstDialogueTriggered) return;

        firstDialogueTriggered = true;
        StartCoroutine(FirstDialogue());
    }

    void TriggerNextDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex <= maxIndex)
        {
            DialogueManager.Instance.StartDialogue(dialogue[dialogueIndex], true);
            //DialogueManager.Instance.AddDialogueQueue(new DialoguePlayer(dialogue[dialogueIndex], true));
        }
    }

    IEnumerator FirstDialogue()
    {
        GameManager.Instance.AllowPlayerToSit(false);
        TriggerNextDialogue();
        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);
        TriggerNextDialogue();
        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);
        TriggerNextDialogue(); // player receives an hourglass
        GameManager.Instance.EnableSkip(true);
        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);
        TriggerNextDialogue();
        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);
        TriggerNextDialogue();
        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);
        TriggerNextDialogue();
        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);

        DialogueManager.Instance.ClearQueue();

        GameManager.Instance.AllowPlayerToSit(true);
        GameManager.Instance.SetCanSkipRoom(true);
        GameManager.Instance.TimerForcedStop(false);
        yield return new WaitForSeconds(1.5f);
        DialogueLoop();
        keymasterScript.SetCanInteract(true);
    }

    public void DialogueLoop()
    {
        if (loopIndex > maxLoopIndex) return;
        DialogueManager.Instance.AddDialogueQueue(new DialoguePlayer(loop[loopIndex], false));
    }

    public void DialogueBox()
    {
        if (boxIndex > maxBoxIndex) return;
        DialogueManager.Instance.AddDialogueQueue(new DialoguePlayer(box[boxIndex], true));
    }

    IEnumerator StartDialogueBox()
    {
        DialogueBox();
        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);
        DialogueLoop();
    }

    IEnumerator QueueDialogueBox()
    {
        yield return new WaitUntil(() => GameManager.Instance.CurrentRoom == 1);
        DialogueBox();
        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);
        DialogueLoop();
    }

    public void TriggerNextLoop()
    {
        loopIndex++;
        DialogueManager.Instance.ClearQueue();

        if (loopIndex <= maxLoopIndex)
        {
            DialogueLoop();
        }
    }

    public void TriggerNextBox()
    {
        boxIndex++;
        DialogueManager.Instance.ClearQueue();

        if (boxIndex <= maxBoxIndex)
        {
            DialogueBox();
        }
    }

    public void TriggerAllDialogue()
    {
        StartCoroutine(StartDialogueBox());
    }

    public void QueueNextAll(bool loopAdd, bool boxAdd)
    {
        if (loopAdd) loopIndex++;
        if (boxAdd) boxIndex++;

        DialogueManager.Instance.ClearQueue();
        StartCoroutine(QueueDialogueBox());
    }

    public void PlayHeadExplodeAnimation()
    {
        pallorAnim.PlayExplodeAnimation();
    }
}
