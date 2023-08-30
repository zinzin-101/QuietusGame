using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public void StartGame()
    {
        LevelManager.Instance.FadeToBlackLoadScene("TestScene"); //change later
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}