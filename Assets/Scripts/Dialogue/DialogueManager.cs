using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public struct DialoguePlayer
{
    public Dialogue dialogue;
    public bool stopTime;

    public DialoguePlayer(Dialogue _dialogue, bool _stopTime)
    {
        dialogue = _dialogue;
        stopTime = _stopTime;
    }
}

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    public static DialogueManager Instance => instance;

    private Queue<string> sentences;
    private Queue<DialoguePlayer> dialogueQueue;

    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject dialogueObject;
    //[SerializeField] float dialogueDelay = 3f;

    private TMP_Text dialogueText;
    private string dialogueName;

    private bool isRunning;
    public bool IsRunning => isRunning;

    private Animator animator;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        dialogueObject.TryGetComponent(out dialogueText);

        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        //dialoguePanel.SetActive(false);

        if (animator != null)
        {
            animator.SetBool("IsRunning", false);
        }
        isRunning = false;

        sentences = new Queue<string>();
        dialogueQueue = new Queue<DialoguePlayer>();
    }

    private void Update()
    {
        if (dialogueQueue.Count != 0 && !isRunning)
        {
            DialoguePlayer dialoguePlayer = dialogueQueue.Dequeue();
            StartDialogue(dialoguePlayer.dialogue, dialoguePlayer.stopTime);
        }
    }

    public void StartDialogue(Dialogue dialogue, bool stopTimer)
    {
        if (stopTimer)
        {
            GameManager.Instance.TimerActive(false);
        }

        //dialoguePanel.SetActive(true);
        isRunning = true;

        if (animator != null)
        {
            animator.SetBool("IsRunning", true);
        }

        dialogueName = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(PrintSentence(sentence));
    }

    void EndDialogue()
    {
        isRunning = false;
        //dialoguePanel.SetActive(false);

        if (animator != null)
        {
            animator.SetBool("IsRunning", false);
        }

        GameManager.Instance.TimerActive(true);
    }

    IEnumerator PrintSentence(string sentence)
    {
        dialogueText.text = dialogueName + ": ";
        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.025f);
        }

        //yield return new WaitForSeconds(dialogueDelay);
        //DisplayNextSentence();
    }

    public void ResetDialogue()
    {
        sentences.Clear();
        dialogueQueue.Clear();
        EndDialogue();
    }

    public void AddDialogueQueue(DialoguePlayer dialogue)
    {
        dialogueQueue.Enqueue(dialogue);
    }
}