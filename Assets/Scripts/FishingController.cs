using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingController : MonoBehaviour
{

    [SerializeField] GameManager gameManager;

    public bool hookOccupied = false;
    public bool casting = false;

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
        Debug.Log("YARRRRR WE BE CARSTINGG");
        GetComponent<Animator>().SetTrigger("Cast");
        gameManager.ToggleMovement();
        casting = true;
    }

    private void Reel()
    {
        GetComponent<Animator>().SetTrigger("Reel");
        gameManager.ToggleMovement();
        casting = false;
    }
}
