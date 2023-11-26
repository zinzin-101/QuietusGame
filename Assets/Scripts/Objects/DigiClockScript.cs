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
    public bool Completed => completed;

    [SerializeField] bool grouped;
    public bool Grouped => grouped;

    private bool disable;

    private bool showing;
    public bool Showing => showing;

    [SerializeField] Dialogue tooHigh;
    public Dialogue TooHigh => tooHigh;

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            currentCode[i] = 0;
        }
        
        panel.SetActive(false);
        completed = false;
        disable = false;
    }

    public void Interact()
    {
        if (disable) return;
        if (completed && !grouped) return;

        panel.SetActive(true);
        showing = true;
        PrintCode();
    }

    public void UnInteract()
    {
        if (completed && !grouped) return;

        panel.SetActive(false);
        showing = false;
    }

    private void PrintCode()
    {
        if (disable) return;
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
        if (disable) return;
        bool correct = true;
        for (int i = 0; i < 4; i++)
        {
            if (currentCode[i] != correctCode[i])
            {
                correct = false;
                break;
            }
        }

        if (grouped)
        {
            completed = correct;
            return;
        }
        if (!correct) return;

        completed = true;

        Instantiate(itemAppear, itemSpawnPos.position, Quaternion.identity);

        //play sound here or something

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
        if (disable) return;
        if (completed && !grouped) return;

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

    public void SetDisable(bool value)
    {
        UnInteract();
        disable = value;
    }

    public void SetActiveGameObject(bool value)
    {
        gameObject.SetActive(value);
    }
}
