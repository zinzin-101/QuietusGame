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

    private bool canFinishTimer = true;
    //private bool timerFinished;

    [SerializeField] float timeDelayBeforeLoadScene = 2f;

    [SerializeField] bool timerActivate; // change later to private

    void Awake()
    {
        timeValue = timeLimit;
        canFinishTimer = true;
        timerActivate = true;
    }

    void Update()
    {
        if (!timerActivate) return;

        if (timeValue > 0 && !timeUp)
        {
            timeValue -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timeValue).ToString("0");
        }

        if (timeValue <= 0 && !timeUp)
        {
            timeUp = true;
            timerText.text = "Time's up!";

            if (canFinishTimer)
            {
                canFinishTimer = false;
                //timerFinished = true;

                StartCoroutine(GameManager.Instance.ChangeRoom());
                //ChangeScene(targetSceneName);
            }
        }
    }

    public void ResetTimer()
    {
        timeValue = timeLimit;
        timerActivate = true;
        timeUp = false;
        canFinishTimer = true;
    }

    public void SetActiveTimer(bool value)
    {
        timerActivate = value;
    }

    void ChangeScene(string sceneName)
    {
        LevelManager.Instance.FadeLoadSceneNoBar(sceneName, timeDelayBeforeLoadScene);
    }
}
