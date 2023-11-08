using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PallorAnimation : MonoBehaviour
{
    private Animator animator;
    private bool isTransitioning = false;
    private Timer timerScript; 

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
    public void PlayExplodeAnimation()
    {
        if (animator != null)
        {
            animator.Play("Explode");
            animator.SetBool("TransitionTo1", true);
        }
    }

}
