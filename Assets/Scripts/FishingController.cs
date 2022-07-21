using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingController : MonoBehaviour
{

    [SerializeField] GameManager gameManager;

    public bool hookOccupied = false;
    public bool casting = false;

    private Animator fishingControllerAnimator;
    private Animator hookBobAnimator;

    private void Start()
    {
        fishingControllerAnimator = GetComponent<Animator>();
        try
        {
            hookBobAnimator = transform.parent.GetComponent<Animator>();
            Debug.Log(hookBobAnimator.gameObject.name);
        }catch(System.Exception e)
        {
            Debug.Log(e);
        }
        
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
        fishingControllerAnimator.SetTrigger("Cast");
        //hookBobAnimator.SetTrigger("Rise");
        gameManager.ToggleMovement();
        casting = true;
    }

    public void Reel()
    {
        fishingControllerAnimator.SetTrigger("Reel");
        hookOccupied = false;
        //StartCoroutine(WaitToLift());
        
    }

    //private IEnumerator WaitToLift()
    //{
    //    yield return new WaitForSeconds(2f);
    //    hookBobAnimator.SetTrigger("Rise");
    //    gameManager.ToggleMovement();
    //    casting = false;
    //}

    //Called at the end of th Reel Animation
    private void ResetAfterReel()
    {
        hookBobAnimator.SetTrigger("Rise");
        gameManager.ToggleMovement();
        casting = false;
    }

    public void BobHook()
    {
        hookBobAnimator.SetTrigger("Bob");
    }

    public void SinkHook()
    {
        hookBobAnimator.SetTrigger("Sink");
    }
}
