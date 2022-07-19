using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBobber : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void BobHook()
    {
        animator.SetTrigger("Bob");
    }

    public void SinkHook()
    {
        animator.SetTrigger("Sink");
    }
}
