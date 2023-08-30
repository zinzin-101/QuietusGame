using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;

    private void Awake()
    {
        pausePanel.SetActive(false);
    }

    public void TogglePause()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
    }

    public void MainMenuButton()
    {
        LevelManager.Instance.FadeToBlackLoadScene("MainMenu");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
