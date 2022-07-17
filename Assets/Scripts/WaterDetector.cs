using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetector : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    [SerializeField] int degreeOfAngle = 20;

    private void FixedUpdate()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(degreeOfAngle, transform.right) * transform.forward, out hit, Mathf.Infinity))
        {
            //Debug.DrawRay(transform.position, Quaternion.AngleAxis(degreeOfAngle, transform.right) * transform.forward * 1000, Color.green);
            //Debug.Log("Hit " + hit.transform.gameObject.tag);

            if(hit.transform.gameObject.tag == "Water")
            {
                gameManager.lookingAtWater = true;
            }
            else
            {
                gameManager.lookingAtWater = false;
            }
        }
        else
        {
            //Debug.DrawRay(transform.position, Quaternion.AngleAxis(degreeOfAngle, transform.right) * transform.forward * 1000, Color.red);
        }
    }

}
