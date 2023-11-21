using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class DigiClockScript : MonoBehaviour
{
    [SerializeField] GameObject itemAppear;
    [SerializeField] Transform itemSpawnPos;
    [SerializeField] int[] correctCode = new int[4];
    private int[] currentCode = new int[4];

    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text codeText;

    private bool completed;

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            currentCode[i] = 0;
        }
        
        panel.SetActive(false);
        completed = false;
    }

    public void Interact()
    {
        if (completed) return;

        panel.SetActive(true);
        PrintCode();
    }

    public void UnInteract()
    {
        if (completed) return;

        panel.SetActive(false);
    }

    private void PrintCode()
    {
        codeText.text = "";
        for (int i = 0; i < 4; i++)
        {
            if (i == 3)
            {
                codeText.text += currentCode[i].ToString();
                continue;
            }
            codeText.text += currentCode[i].ToString() + " ";
        }
    }

    private void CheckCode()
    {
        bool correct = true;
        for (int i = 0; i < 4; i++)
        {
            if (currentCode[i] != correctCode[i])
            {
                correct = false;
                break;
            }
        }

        if (!correct) return;

        Instantiate(itemAppear, itemSpawnPos.position, Quaternion.identity);

        //play sound here or something

        completed = true;
        panel.SetActive(false);
        Destroy(gameObject);
    }

    public void ChangeDigit0(bool add)
    {
        ChangeDigit(0, add);
    }

    public void ChangeDigit1(bool add)
    {
        ChangeDigit(1, add);
    }

    public void ChangeDigit2(bool add)
    {
        ChangeDigit(2, add);
    }

    public void ChangeDigit3(bool add)
    {
        ChangeDigit(3, add);
    }

    private void ChangeDigit(int index, bool add)
    {
        if (completed) return;

        switch (add)
        {
            case true:
                currentCode[index]++;
                break;

            case false:
                currentCode[index]--;
                break;
        }

        if (currentCode[index] > 9)
        {
            currentCode[index] = 0;
        }
        else if (currentCode[index] < 0)
        {
            currentCode[index] = 9;
        }

        PrintCode();
        CheckCode();
    }
}
