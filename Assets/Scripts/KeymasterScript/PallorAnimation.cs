using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PallorAnimation : MonoBehaviour
{
    //private Animator animator;
    //private bool isTransitioning = false;

    //private void Start()
    //{
    //    animator = GetComponent<Animator>();
    //    StartCoroutine(PlayAnimationSequence());
    //}

    //private IEnumerator PlayAnimationSequence()
    //{
    //    // Play the first animation for 30 seconds
    //    animator.SetBool("TransitionTo2", false);
    //    yield return new WaitForSeconds(30f);

    //    // Trigger the transition to the second animation
    //    animator.SetBool("TransitionTo2", true);

    //    // Wait for the transition to complete (you may need to adjust the time)
    //    yield return new WaitForSeconds(1f);

    //    // Play the third animation for 20 seconds
    //    animator.SetBool("TransitionTo3", true);
    //    yield return new WaitForSeconds(20f);

    //    // Trigger the transition to the fourth animation
    //    animator.SetBool("TransitionTo4", true);

    //    // Wait for the transition to complete
    //    yield return new WaitForSeconds(1f);

    //    // Play the fifth animation for 10 seconds
    //    animator.SetBool("TransitionTo5", true);
    //    yield return new WaitForSeconds(10f);

    //    // Wait for 60 seconds before playing the sixth animation
    //    yield return new WaitForSeconds(1f);

    //    // Play the sixth animation
    //    animator.SetBool("TransitionTo6", true);
    //}

    private Animator animator;
    private bool isTransitioning = false;
    private Timer timerScript; // Reference to the Timer script

    // The name of the GameObject containing the Timer script
    //public string timerGameObjectName = "TimerGameObject";

    private float transitionDuration = 1f;

    private void Start()
    {
        animator = GetComponent <Animator>();

        // Find the GameObject with the Timer script using its name
        timerScript = FindFirstObjectByType<Timer>();
        if (timerScript == null)
        {
            Debug.LogError("Timer GameObject not found.");
        }

        StartCoroutine(PlayAnimationSequence());
    }
    private IEnumerator PlayAnimationSequence()
    {
        while (true)
        {
            if (!isTransitioning)
            {
                if (timerScript != null)
                {
                    float timerValue = timerScript.GetTimeValue();

                    // Determine the current animation based on the timer value
                    if (timerValue <= 30f)
                    {
                        animator.SetBool("TransitionTo2", true);
                        animator.SetBool("TransitionTo3", true);

                        animator.SetBool("TransitionTo6", false);
                        animator.SetBool("TransitionTo1", false);
                    }
                    if (timerValue <= 10f)
                    {
                        animator.SetBool("TransitionTo4", true);
                        animator.SetBool("TransitionTo5", true);

                        animator.SetBool("TransitionTo2", false);
                        animator.SetBool("TransitionTo3", false);
                    }
                    if (timerValue <= 0)
                    {
                        animator.SetBool("TransitionTo6",true);
                        animator.SetBool("TransitionTo1",true);
                        
                        animator.SetBool("TransitionTo4", false);
                        animator.SetBool("TransitionTo5", false);
                        
                    }
                }
            }
            else
            {
                yield return new WaitForSeconds(transitionDuration);
            }
            yield return null;
        }
    }

    private void PlayAnimation(string transitionParameterName)
    {
        isTransitioning = true;
        animator.SetBool(transitionParameterName, true);
        StartCoroutine(CompleteTransition(transitionParameterName));
    }

    private IEnumerator CompleteTransition(string transitionParameterName)
    {
        yield return new WaitForSeconds(transitionDuration);
        animator.SetBool(transitionParameterName, false);
        isTransitioning = false;
    }
}
