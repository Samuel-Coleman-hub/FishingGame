using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingController : MonoBehaviour
{

    [SerializeField] GameManager gameManager;

    public bool hookOccupied = false;
    public bool casting = false;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Fishing()
    {
        if (casting)
        {
            Reel();
            
        }
        else
        {
            Cast();
        }
    }

    private void Cast()
    {
        animator.SetTrigger("Cast");
        gameManager.ToggleMovement();
        casting = true;
    }

    private void Reel()
    {
        animator.SetTrigger("Reel");
        gameManager.ToggleMovement();
        casting = false;
    }
}
