using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTextScript : MonoBehaviour
{
    [SerializeField] float delay = 1.5f;
    [SerializeField] Animator animator;

    IEnumerator Start()
    {
        animator.SetBool("Start", true);
        yield return new WaitForSeconds(delay);
        animator.SetBool("Start", false);
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
