using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    private Transform hook;
    [SerializeField] LayerMask whatIsSeaBed, whatIsPlayer;

    //Patrol Variables
    [SerializeField] Vector3 swimPoint;
    private bool swimPointSet;
    [SerializeField] float swimPointRange;

    public float sightRange;
    public bool hookInSightRange;

    private void Awake()
    {
        hook = GameObject.FindGameObjectWithTag("Hook").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //check sightrange
        hookInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!hookInSightRange)
        {
            Patrolling();
        }
        else
        {
            SwimToHook();
        }
    }

    private void Patrolling()
    {
        if (!swimPointSet)
        {
            FindSwimPoint();
        }
        else
        {
            agent.SetDestination(swimPoint);
        }

        Vector3 distanceToSwimPoint = transform.position - swimPoint;

        if (distanceToSwimPoint.magnitude < 1f)
        {
            swimPointSet = false;
        }
    }

    private void FindSwimPoint()
    {
        float randomZ = Random.Range(-swimPointRange, swimPointRange);
        float randomX = Random.Range(-swimPointRange, swimPointRange);

        swimPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(swimPoint, - transform.up, 2f, whatIsSeaBed))
        {
            swimPointSet = true;
        }
    }

    private void SwimToHook()
    {
        agent.SetDestination(hook.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}
