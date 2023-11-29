using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    private void Start()
    {
        SoundManager.PlaySound(SoundManager.Sound.BGM);
    }
    public async void StartGame()
    {
        SoundManager.PlaySound(SoundManager.Sound.Start);
        await Task.Delay(2000);
        LevelManager.Instance.FadeToBlackLoadScene("Gameplay"); 
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}