using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    [SerializeField] string[] hangmanDialogue;

    private void Awake()
    {
        textObject.SetActive(false);

        started = false;

        answerIndex = 0;
        maxIndex = answers.Length;
    }



    private void Update()
    {
        if (started)
        {
            if (tries <= 0)
            {
                StopGame();
                return;
            }

            if (Input.anyKeyDown)
            {
                string keyPressed;
                keyPressed = Input.inputString;

                if (keyPressed == "\b")
                {
                    if (keyEntered > 0)
                    {
                        keyEntered--;
                        playerAnswer.Remove(playerAnswer.Length - 1);
                    }
                }
                else
                {
                    if (keyEntered < currentMaxAnswerLength)
                    {
                        keyEntered++;
                        playerAnswer += keyPressed;
                    }
                }

                PrintText();

                if (playerAnswer.Length >= currentMaxAnswerLength)
                {
                    if (playerAnswer.ToLower() == currentAnswer.ToLower())
                    {
                        NextWord();
                    }
                    else
                    {
                        GameActive();
                        tries--;
                    }
                }
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
        tries = maxTries;
        answerIndex = 0;
        GameActive();
    }

    public void GameActive()
    {
        currentAnswer = answers[answerIndex];
        currentMaxAnswerLength = answers[answerIndex].Length;
        keyEntered = 0;

        playerAnswer = "";
        hangmanText.text = "";

        for (int i = 0; i < currentMaxAnswerLength; i++)
        {
            hangmanText.text += "_ ";
        }

        StartCoroutine(PlayHint());
    }

    public void NextWord()
    {
        answerIndex++;
        if (answerIndex > maxIndex) answerIndex = maxIndex;
        GameActive();
    }

    public void StopGame()
    {
        started = false;
        textObject.SetActive(false);
        GameManager.Instance.AllowPlayerToMove(true);
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

    void PrintText()
    {
        for (int i = 1; i <= currentMaxAnswerLength; i++)
        {
            hangmanText.text = "";
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
