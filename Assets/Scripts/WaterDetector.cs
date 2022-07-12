using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetector : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            gameManager.lookingAtGround = true;
            Debug.Log("DETECTOR Looking at ground");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Ground")
        {
            gameManager.lookingAtGround = false;
            Debug.Log("DETECTOR Not looking at ground");
        }
    }

}
