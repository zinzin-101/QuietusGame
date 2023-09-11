using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance => instance;

    [SerializeField] GameObject loader;
    [SerializeField] UnityEngine.UI.Image progressBar;
    private float target;

    [SerializeField] GameObject fadeCanvas;
    [SerializeField] Image fade;
    private float currentFade;
    private float targetFade;
    private Color defaultFade;
    [SerializeField] float fadeSpeed = 3f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        defaultFade = fade.color;
    }
    
    public void FadeToBlack()
    {
        fadeCanvas.SetActive(true);

        Color _color = defaultFade;
        _color.a = 0f;
        currentFade = 0f;
        targetFade = 1f;
        fade.color = _color;
    }

    public void FadeFromBlack()
    {
        fadeCanvas.SetActive(true);

        Color _color = defaultFade;
        _color.a = 1f;
        currentFade = 1f;
        targetFade = 0f;
        fade.color = _color;
    }

    public async void LoadScene(string sceneName)
    {
        target = 0f;
        progressBar.fillAmount = 0f;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        loader.SetActive(true);

        do
        {
            await Task.Delay(100);
            target = scene.progress;
        }
        while (scene.progress < 0.9f);

        await Task.Delay(1000);

        scene.allowSceneActivation = true;
        loader.SetActive(false);

        FadeFromBlack();

        do
        {
            await Task.Delay(1);
        }
        while (fade.color.a != 0f);
        fadeCanvas.SetActive(false);
    }

    public async void DelayLoadScene(string sceneName, float delay)
    {
        await Task.Delay((int)(delay * 1000));
        LoadScene(sceneName);
    }

    public async void FadeToBlackLoadScene(string sceneName)
    {
        FadeToBlack();

        do
        {
            await Task.Delay(1);
        }
        while (fade.color.a != 1f);

        LoadScene(sceneName);
    }

    public async void FadeLoadSceneNoBar(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        FadeToBlack();

        do
        {
            await Task.Delay(1);
        }
        while (fade.color.a != 1f);

        scene.allowSceneActivation = true;

        FadeFromBlack();

        do
        {
            await Task.Delay(1);
        }
        while (fade.color.a != 0f);

        fadeCanvas.SetActive(false);
    }

    public async void FadeLoadSceneNoBar(string sceneName, float delay)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        await Task.Delay((int)(delay * 1000));

        FadeToBlack();

        do
        {
            await Task.Delay(1);
        }
        while (fade.color.a != 1f);

        scene.allowSceneActivation = true;

        FadeFromBlack();

        do
        {
            await Task.Delay(1);
        }
        while (fade.color.a != 0f);

        fadeCanvas.SetActive(false);
    }

    public async void NormalFadeInOut(float delay)
    {
        await Task.Delay((int)(delay * 1000));

        FadeToBlack();

        do
        {
            await Task.Delay(1);
        }
        while (fade.color.a != 1f);

        FadeFromBlack();

        do
        {
            await Task.Delay(1);
        }
        while (fade.color.a != 0f);

        fadeCanvas.SetActive(false);
    }

    public async Task NormalFadeIn(float delay)
    {
        await Task.Delay((int)(delay * 1000));

        FadeToBlack();

        do
        {
            await Task.Delay(1);
        }
        while (fade.color.a != 1f);
    }

    public async Task NormalFadeOut()
    {
        FadeFromBlack();

        do
        {
            await Task.Delay(1);
        }
        while (fade.color.a != 0f);

        fadeCanvas.SetActive(false);
    }

    private void Update()
    {
        progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, 3 * Time.deltaTime);

        Color _color = fade.color;
        currentFade = Mathf.MoveTowards(currentFade, targetFade, fadeSpeed * Time.deltaTime);
        _color.a = currentFade;
        fade.color = _color;
    }
    
    public void SetFadeActive(bool value)
    {
        fadeCanvas.SetActive(value);
    }
}
