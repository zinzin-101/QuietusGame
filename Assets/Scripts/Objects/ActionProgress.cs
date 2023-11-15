using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionProgress : MonoBehaviour
{
    private bool actionFinished;
    public bool ActionFinished => actionFinished;

    [SerializeField] float timeRequired = 4f;
    private float timePassed;
    private bool activate;
    public bool Activate => activate;

    [SerializeField] GameObject progressBar, progressBarBG;
    private float defaultScale, currentScale;

    private void Awake()
    {
        defaultScale = progressBar.transform.localScale.x;
        currentScale = 0f;
    }

    private void Start()
    {
        timePassed = 0f;
        actionFinished = false;
        activate = false;

        HideBar();
    }

    private void Update()
    {
        if (actionFinished) return;

        if (activate)
        {
            timePassed += Time.deltaTime;
            
            if (timePassed >= timeRequired)
            {
                timePassed = timeRequired;
                activate = false;
                actionFinished = true;
            }
        }
        else
        {
            timePassed = 0f;
        }
    }

    private void FixedUpdate()
    {
        currentScale = (timePassed / timeRequired) * defaultScale;
        
        Vector3 newScale = progressBar.transform.localScale;
        newScale.x = currentScale;
        progressBar.transform.localScale = newScale;
    }

    public void ShowBar()
    {
        progressBar.SetActive(true);
        progressBarBG.SetActive(true);
    }

    public void HideBar()
    {
        progressBar.SetActive(false);
        progressBarBG.SetActive(false);
    }

    public void ResetProgressBar()
    {
        if (actionFinished) return;

        timePassed = 0f;
        actionFinished = false;
        activate = false;
        HideBar();
    }

    public void SetProgressActive(bool value)
    {
        activate = value;
    }
}
