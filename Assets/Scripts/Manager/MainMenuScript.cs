using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public async void StartGame()
    {
        await Task.Delay(2000);
        LevelManager.Instance.FadeToBlackLoadScene("Gameplay"); //change later
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}