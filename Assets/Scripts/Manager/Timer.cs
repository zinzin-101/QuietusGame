using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float timeLimit = 60;
    private float timeValue;
    private bool timeUp = false;

    void Awake()
    {
        timeValue = timeLimit;
    }

    void Update()
    {
        if (timeValue > 0 && !timeUp)
        {
            timeValue -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timeValue).ToString("0");
        }

        if (timeValue <= 0 && !timeUp)
        {
            timeUp = true;
            timerText.text = "Time's up!";
        }
    }
}
