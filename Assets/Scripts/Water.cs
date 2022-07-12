using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gameManager.lookingAtWater = true;
            Debug.Log("WATER Looking at water");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            gameManager.lookingAtWater = false;
            Debug.Log("WATER Not looking at water");
        }
    }
}
