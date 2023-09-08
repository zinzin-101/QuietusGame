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

    private bool canSwitchScene = true;
    [SerializeField] string targetSceneName;
    [SerializeField] float timeDelayBeforeLoadScene = 2f;

    void Awake()
    {
        timeValue = timeLimit;
        canSwitchScene = true;
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

            if (canSwitchScene)
            {
                canSwitchScene = false;
                ChangeScene(targetSceneName);
            }
        }
    }

    void ChangeScene(string sceneName)
    {
        LevelManager.Instance.FadeLoadSceneNoBar(sceneName, timeDelayBeforeLoadScene);
    }
}
