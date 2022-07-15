using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishController : MonoBehaviour
{
    [SerializeField] float idleTimeMin = 1f;
    [SerializeField] float idleTimeMax = 6f;

    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;
    private Animator animator;

    private Transform hook;
    private FishingController fishingController;
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
        fishingController = hook.gameObject.GetComponent<FishingController>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        animator = GetComponent<Animator>();

        animator.SetTrigger("Swim");
    }

    private void Update()
    {
        hookInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);


        if (!hookInSightRange)
        {
            Patrolling();
        }
        else if(fishingController.casting && !fishingController.hookOccupied)
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
                StartCoroutine(WaitAtPoint());
            }
    }

    private void FindSwimPoint()
    {
        float randomZ = Random.Range(-swimPointRange, swimPointRange);
        float randomX = Random.Range(-swimPointRange, swimPointRange);

        swimPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(swimPoint, out hit, 1f, NavMesh.AllAreas))
        {
            swimPointSet = true;
        }
    }

    private void SwimToHook()
    {
        fishingController.hookOccupied = true;
        agent.SetDestination(new Vector3(hook.position.x, transform.position.y, hook.position.z));
    }

    private IEnumerator WaitAtPoint()
    {
        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(Random.Range(idleTimeMin, idleTimeMax));
        animator.SetTrigger("Swim");
        swimPointSet = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}
