using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EndScript : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;

    private void Awake()
    {
        StartCoroutine(Run());
    }
    IEnumerator Run()
    {
        yield return new WaitUntil(() => videoPlayer.waitForFirstFrame);
        yield return new WaitUntil(() => videoPlayer.isPlaying);
        yield return new WaitUntil(() => !videoPlayer.isPlaying);
        LevelManager.Instance.FadeToBlackLoadScene("MainMenu");
    }
}
