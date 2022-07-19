using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishController : MonoBehaviour
{
    //Fish Biting Hook Variables
    [Header("Fish Biting Settings")]
    [SerializeField] float distanceBehindFish = 2.5f;
    [SerializeField] float bitingSpeed = 1f;
    private Vector3[] bitingPositions = new Vector3[2];
    private int bitingState = 0;
    private bool biting = false;
    private Transform hook;

    //NavMesh movement settings
    [Header("NavMeshAgent Movement Settings")]
    [SerializeField] float idleTimeMin = 1f;
    [SerializeField] float idleTimeMax = 6f;
    [SerializeField] Vector3 swimPoint;
    [SerializeField] float swimPointRange;
    [SerializeField] LayerMask whatIsSeaBed, whatIsPlayer;
    private bool swimPointSet;
    public float sightRange;
    public bool hookInSightRange;
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;

    //GameObject Properties
    private Animator animator;
    private Rigidbody rb;
    private FishingController fishingController;

    private void Awake()
    {
        hook = GameObject.FindGameObjectWithTag("Hook").transform;
        fishingController = hook.gameObject.GetComponent<FishingController>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        animator.SetTrigger("Swim");
    }

    private void Update()
    {
        hookInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!biting)
        {
            if (!hookInSightRange)
            {
                Patrolling();
            }
            else if (fishingController.casting) //need to work out new way to check if occupied hook)
            {
                SwimToHook();
            }
        }
        else
        {
            BitingHook();
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
        agent.SetDestination(hook.position);

        Vector3 distanceToHook = transform.position - hook.transform.position;

        if (distanceToHook.magnitude < 1f)
        {

            Vector3 behindFish = transform.position - (transform.forward * distanceBehindFish);
            Vector3 hookPos = new Vector3(hook.transform.position.x, transform.position.y, hook.transform.position.z);
            bitingPositions[0] = behindFish;
            bitingPositions[1] = hookPos;
            agent.enabled = false;

            biting = true;
        }
    }

    private void BitingHook()
    {
        Debug.Log("Biting");
        //biting = true;
        //animator.SetTrigger("Biting");

        if(Vector3.Distance(bitingPositions[bitingState], transform.position) < 1f)
        {
            if(bitingState == 0)
            {
                bitingState = 1;
            }
            else
            {
                fishingController.BobHook();
                bitingState = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, bitingPositions[bitingState], Time.deltaTime * bitingSpeed);

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
