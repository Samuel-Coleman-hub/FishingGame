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
    [SerializeField] int minBites = 2, maxBites = 5;
    private Vector3[] bitingPositions = new Vector3[2];
    private int bitingState = 0;
    private int biteCounter = 0;
    private int totalBites = 6;
    private bool biting = false;
    private Transform hook;
    private bool thisFishToHook = false;

    [Header("Fish Catching Settings")]
    [SerializeField] float waitTimeToCatch = 3f;
    private bool caught = false;
    private HingeJoint hookHinge;

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
    //private HookBobber bobber;

    //stopWatch variables
    private float stopWatch;
    private bool stopWatchOn;

    //Other Variables
    private float lastCallTime = 1;
    private bool scared = false;

    private void Awake()
    {
        hook = GameObject.FindGameObjectWithTag("Hook").transform;
        fishingController = GameObject.FindGameObjectWithTag("Player").GetComponent<FishingController>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        hookHinge = hook.GetComponent<HingeJoint>();

        animator.SetTrigger("Swim");
    }

    private void Update()
    {
        hookInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!biting && !caught && !scared)
        {
            if (!hookInSightRange && (Time.time - lastCallTime >= 0.2f))
            {
                Patrolling();
            }
            else if (fishingController.casting && (!fishingController.hookOccupied || thisFishToHook)) //need to work out new way to check if occupied hook)
            {
                SwimToHook();
            }
        }
        else if(biting && !scared)
        {
            BitingHook();
        }

        if (stopWatchOn)
        {
            stopWatch += Time.deltaTime;
        }
        
    }

    private void Patrolling()
    {
        lastCallTime = Time.time;

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
        thisFishToHook = true;
        agent.SetDestination(hook.position);

        Vector3 distanceToHook = transform.position - hook.transform.position;

        if (distanceToHook.magnitude < 1f)
        {
            StartBitingSequence();
        }
    }

    private void StartBitingSequence()
    {
        fishingController.BobHook();
        Vector3 behindFish = transform.position - (transform.forward * distanceBehindFish);
        Vector3 hookPos = new Vector3(hook.transform.position.x, transform.position.y, hook.transform.position.z);

        biteCounter = 0;
        totalBites = Random.Range(minBites, maxBites);

        bitingPositions[0] = behindFish;
        bitingPositions[1] = hookPos;
        agent.enabled = false;

        biting = true;
    }

    private void BitingHook()
    {
        if(Vector3.Distance(bitingPositions[bitingState], transform.position) < 1f)
        {
            if(bitingState == 0)
            {
                bitingState = 1;
            }
            else
            {
                bitingState = 0;
                biteCounter++;

                if (biteCounter <= totalBites)
                {
                    //bobber.BobHook();
                    fishingController.BobHook();
                }
                else
                {
                    FishHooked();
                }
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, bitingPositions[bitingState], Time.deltaTime * bitingSpeed);
    }

    private void FishHooked()
    {
        caught = true;
        biting = false;
        agent.enabled = false;
        fishingController.SinkHook();

        StartCoroutine(CatchFish());
    }

    private IEnumerator CatchFish()
    {
        bool done = false;

        StartStopWatch();

        while (!done)
        {
            if (Input.GetButtonDown("Fish"))
            {
                done = true;
                StopStopWatch();
                ReelInFish();
            }
            else if(stopWatch >= waitTimeToCatch)
            {
                done = true;
                StopStopWatch();
                FishEscape();
            }
            yield return null;
        }
    }

    private void ReelInFish()
    {
        animator.enabled = false;
        hookHinge.connectedBody = rb;
        fishingController.hookOccupied = false;
        StartCoroutine(WaitToKill(3f));
    }

    //Only for development purposes
    private IEnumerator WaitToKill(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    public void FishEscape()
    {
        //if (hookInSightRange)
        //{
            scared = true;
            agent.enabled = true;
            FindSwimPoint();
            agent.speed *= 4;
            agent.SetDestination(swimPoint);
            fishingController.Reel();
            //Will need to fade out in final game
            StartCoroutine(WaitToKill(3f));
        //}
    }

    private void StartStopWatch()
    {
        stopWatch = 0;
        stopWatchOn = true;
    }

    private void StopStopWatch()
    {
        stopWatchOn = false;
        stopWatch = 0;
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
