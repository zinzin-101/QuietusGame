using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PallorMortisScript1 : MonoBehaviour
{   
    [SerializeField] Dialogue[] dialogue;
    private int dialogueIndex;
    private int maxIndex;

    [SerializeField] Dialogue[] loop;
    private int loopIndex;
    private int maxLoopIndex;

    [SerializeField] Dialogue[] box;
    private int boxIndex;
    private int maxBoxIndex;

    [SerializeField] Dialogue score;
    [SerializeField] Dialogue finalBoom;
    [SerializeField] Dialogue v1, v2;
    [SerializeField] Item scoreReport;
    private Collider2D col;
    [SerializeField] GameObject spriteObject;
    [SerializeField] GameObject bear;
    [SerializeField] GameObject rope;
    private bool canCheckInventory;
    public bool CanCheckInventory => canCheckInventory;
    private bool validScore;

    private bool finalActivated;
    public bool FinalActivated;

    private bool firstDialogueTriggered;

    [SerializeField] PallorAnimation pallorAnim;

    private void Awake()
    {
        TryGetComponent(out col);
        if (col != null)
        {
            col.enabled = true;
        }

        finalActivated = false;
        spriteObject.SetActive(true);
        rope.SetActive(false);
        bear.SetActive(true);

        dialogueIndex = 0;
        maxIndex = dialogue.Length - 1;

        loopIndex = 0;
        maxLoopIndex = loop.Length - 1;

        boxIndex = 0;
        maxBoxIndex = box.Length - 1;

        firstDialogueTriggered = false;
        
        canCheckInventory = false;
        validScore = false;
    }

    private IEnumerator Start()
    {
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
        yield return new WaitUntil(() => GameManager.Instance.CurrentRoom == 1 && GameManager.Instance.CanStartDialogue);
        DialogueBox();
        yield return new WaitUntil(() => (!DialogueManager.Instance.IsRunning && GameManager.Instance.CanStartDialogue));
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
        if (finalActivated) return;

        StartCoroutine(QueueDialogueBox());
    }


    public void QueueNextAll(bool loopAdd, bool boxAdd)
    {
        if (loopAdd) loopIndex++;
        if (boxAdd) boxIndex++;

        DialogueManager.Instance.ClearQueue();
        StartCoroutine(QueueDialogueBox());
    }

    public void NextAll()
    {
        loopIndex++;
        boxIndex++;
    }

    public void PlayHeadExplodeAnimation()
    {
        pallorAnim.PlayExplodeAnimation();
    }

    public void ScoreAcquired()
    {
        GameManager.Instance.TimerActive(false);
        GameManager.Instance.EnableSkip(false);
        DialogueManager.Instance.StartDialogue(score, true);
        canCheckInventory = true;
        finalActivated = true;
        StartCoroutine(FinalSceneStart());
    }

    IEnumerator FinalSceneStart()
    {
        yield return new WaitUntil(() => validScore);
        DialogueManager.Instance.StartDialogue(finalBoom, true);
        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);
        GameManager.Instance.NextRoomButtonAnimation();
        yield return new WaitUntil(() => GameManager.Instance.CanStartDialogue);
        DialogueManager.Instance.StartDialogue(v1, true);
        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);
        GameManager.Instance.NextRoomButton();
        yield return new WaitUntil(() => GameManager.Instance.CanStartDialogue);
        DialogueManager.Instance.StartDialogue(v2, true);

        spriteObject.SetActive(false);
        col.enabled = false;
        if (bear != null) bear.SetActive(false);
        if (rope != null) rope.SetActive(true);

        yield return new WaitUntil(() => !DialogueManager.Instance.IsRunning);
        GameManager.Instance.NextRoomButton();
        GameManager.Instance.TimerActive(false);
    }

    public void CheckForScore()
    {
        if (!canCheckInventory || validScore) return;
        canCheckInventory = false;

        foreach (Item item in InventoryManager.Instance.Items)
        {
            if (item == scoreReport)
            {
                canCheckInventory = true;
                validScore = true;
                return;
            }
        }
        canCheckInventory = true;
        return;
    }
}
