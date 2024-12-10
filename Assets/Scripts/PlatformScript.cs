using System;
using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    private Animator animator;

    // Variable to control the duration of the platform holding at the top
    public float holdDuration = 7f;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(PlatformCycle());
    }

    private IEnumerator PlatformCycle()
    {
        while (true)
        {
            // Start at the bottom
            animator.SetInteger("PlatformState", 0);
            yield return new WaitForSeconds(2f); // Wait at the bottom

            // Rise
            animator.SetInteger("PlatformState", 1);
            yield return new WaitForSeconds(1f); // Time for rising animation

            // Hold at the top
            animator.SetInteger("PlatformState", 2);
            yield return new WaitForSeconds(holdDuration); // Dynamic hold duration

            // Descend
            animator.SetInteger("PlatformState", 3);
            yield return new WaitForSeconds(1f); // Time for descending animation
        }
        
    }

}
