using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TimerAnimationController : MonoBehaviour
{
    private Animator animator;
    private Timer timerScript;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        timerScript = FindFirstObjectByType<Timer>();
        if (timerScript == null)
        {
            Debug.LogError("Timer GameObject not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timerScript.GetTimeValue() > 0)
        {
            animator.SetBool("TimeOut",false);
        }
        if (timerScript.GetTimeValue() <= 0) 
        {
            animator.SetBool("TimeOut", true);
        }
    }
}
