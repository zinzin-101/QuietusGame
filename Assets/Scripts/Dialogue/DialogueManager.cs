using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    public static DialogueManager Instance => instance;

    private Queue<string> sentences;

    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject dialogueObject;
    [SerializeField] GameObject dialogueNameObject;

    private TMP_Text dialogueText;
    private TMP_Text nameText;

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
        dialogueNameObject.TryGetComponent(out nameText);

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
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //dialoguePanel.SetActive(true);
        isRunning = true;

        if (animator != null)
        {
            animator.SetBool("IsRunning", true);
        }

        nameText.text = dialogue.name;

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
    }

    IEnumerator PrintSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}
