using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HangmanScript : MonoBehaviour
{
    [SerializeField] GameObject textObject;
    [SerializeField] TMP_Text hangmanText;
    [SerializeField] TMP_Text hintText;
    private bool started;

    [SerializeField] string[] answers;
    private string currentAnswer, playerAnswer;
    private int answerIndex, maxIndex;
    private int currentMaxAnswerLength;
    private int keyEntered;

    [SerializeField] int maxTries = 6;
    private int tries;

    [TextArea(3, 10)]
    [SerializeField] string[] hangmanDialogue;
    [TextArea(3, 10)]
    [SerializeField] string[] rightDialogue;
    [TextArea(3, 10)]
    [SerializeField] string[] wrongDialogue;

    private bool isActive;
    public bool IsActive => isActive;

    [SerializeField] Sprite[] hangmanPages;
    [SerializeField] Image hangmanImage;

    private void Awake()
    {
        textObject.SetActive(false);

        started = false;

        answerIndex = 0;
        maxIndex = answers.Length - 1;

        isActive = false;
    }

    private void Update()
    {
        if (started)
        {
            if (tries < 0)
            {
                if (answerIndex == maxIndex)
                {
                    GameEnd();
                    return;
                }
                else
                {
                    StartCoroutine(End());
                    return;
                }
            }

            if (Input.anyKeyDown)
            {
                string keyPressed;
                keyPressed = Input.inputString;
                if (Regex.IsMatch(keyPressed, "^[abcdefghijklmnopkrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ']"))
                {
                    if (keyEntered < currentMaxAnswerLength)
                    {
                        keyEntered++;
                        playerAnswer += keyPressed;
                    }
                }
                else if (keyPressed == "\b")
                {
                    if (keyEntered > 0)
                    {
                        keyEntered--;
                        playerAnswer = playerAnswer.Remove(playerAnswer.Length - 1);
                    }
                }

                if (playerAnswer.Length >= currentMaxAnswerLength)
                {
                    if (playerAnswer.ToLower() == currentAnswer.ToLower())
                    {
                        NextWord();
                    }
                    else
                    {
                        GameActive(false);
                        tries--;
                    }
                }

                PrintText();

            }
            else
            {
                PrintText();
            }
        }
    }

    public void StartRoom()
    {
        textObject.SetActive(true);

        started = true;
        isActive = true;
        tries = maxTries;
        answerIndex = 0;
        GameActive();
    }

    public void GameActive()
    {
        hangmanImage.sprite = hangmanPages[maxTries - tries];

        currentAnswer = answers[answerIndex];
        currentMaxAnswerLength = answers[answerIndex].Length;
        keyEntered = 0;

        playerAnswer = "";
        hangmanText.text = "";

        for (int i = 0; i < currentMaxAnswerLength; i++)
        {
            hangmanText.text += "_ ";
        }
        StopAllCoroutines();
        StartCoroutine(PlayHint());
    }

    public void GameActive(bool correct)
    {
        hangmanImage.sprite = hangmanPages[maxTries - tries];

        currentAnswer = answers[answerIndex];
        currentMaxAnswerLength = answers[answerIndex].Length;
        keyEntered = 0;

        playerAnswer = "";
        hangmanText.text = "";

        for (int i = 0; i < currentMaxAnswerLength; i++)
        {
            hangmanText.text += "_ ";
        }
        StopAllCoroutines();
        StartCoroutine(PlayHint(correct));
    }

    public void NextWord()
    {
        answerIndex++;
        if (answerIndex > maxIndex) answerIndex = maxIndex;
        GameActive(true);
    }

    public void GameEnd()
    {
        started = false;
        StopAllCoroutines();
        StartCoroutine(TriggerEnd());
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(2f);
        StopAllCoroutines();
        StopGame();
    }

    IEnumerator TriggerEnd()
    {
        yield return new WaitForSeconds(3f);
        hintText.text = "";

        foreach (char letter in rightDialogue[answerIndex])
        {
            hintText.text += letter;
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(2f);
        print("game end");
        StopGame();
    }

    public void StopGame()
    {
        started = false;
        isActive = false;
        textObject.SetActive(false);
        GameManager.Instance.AllowPlayerToMove(true);
        StopAllCoroutines();
        StartCoroutine(GameManager.Instance.ChangeRoom());
    }

    IEnumerator PlayHint()
    {
        hintText.text = "";
        foreach (char letter in hangmanDialogue[answerIndex])
        {
            hintText.text += letter;
            yield return new WaitForSeconds(0.025f);
        }
    }

    IEnumerator PlayHint(bool correct)
    {
        hintText.text = "";
        switch (correct)
        {
            case true:
                foreach (char letter in rightDialogue[answerIndex-1])
                {
                    hintText.text += letter;
                    yield return new WaitForSeconds(0.025f);
                }
                break;
            case false:
                foreach (char letter in wrongDialogue[answerIndex])
                {
                    hintText.text += letter;
                    yield return new WaitForSeconds(0.025f);
                }
                break;
        }

        yield return new WaitForSeconds(3f);
        hintText.text = "";

        foreach (char letter in hangmanDialogue[answerIndex])
        {
            hintText.text += letter;
            yield return new WaitForSeconds(0.025f);
        }
    }

    void PrintText()
    {
        hangmanText.text = "";
        for (int i = 1; i <= currentMaxAnswerLength; i++)
        {
            if (i <= keyEntered)
            {
                hangmanText.text += playerAnswer[i - 1];
            }
            else
            {
                hangmanText.text += "_ ";

            }
        }
    }
}
