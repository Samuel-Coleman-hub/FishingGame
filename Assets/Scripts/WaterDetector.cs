using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetector : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    [SerializeField] int degreeOfAngle = 20;

    private float lastCallTime = 1;
    private RaycastHit hit;

    private void FixedUpdate()
    {
        if(Time.time - lastCallTime >= 0.2f)
        {
            RayCast();
        }
    }

    private void RayCast()
    {
        lastCallTime = Time.time;
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(degreeOfAngle, transform.right) * transform.forward, out hit, Mathf.Infinity))
        {

            if (hit.transform.gameObject.tag == "Water")
            {
                gameManager.lookingAtWater = true;
                Debug.Log("Looking at water");
            }
            else
            {
                gameManager.lookingAtWater = false;
                Debug.Log("Not looking at water");
            }

            Debug.DrawRay(transform.position, Quaternion.AngleAxis(degreeOfAngle, transform.right) * transform.forward, Color.green);
        }
    }
}
