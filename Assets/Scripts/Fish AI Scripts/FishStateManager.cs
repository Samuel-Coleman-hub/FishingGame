using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishStateManager : MonoBehaviour
{
    //References the active state in the state machine
    FishBaseState currentState;

    public FishPatrollingState patrollingState = new FishPatrollingState();
    public FishSwimToHookState swimToHookState = new FishSwimToHookState();
    public FishBitingState bitingState = new FishBitingState();
    public FishHookedState hookedState = new FishHookedState();
    public FishReelState reelState = new FishReelState();
    public FishEscapeState escapeState = new FishEscapeState();

    //Fish Variables
    [SerializeField] public LayerMask whatIsSeaBed, whatIsPlayer;
    public FishingManager fishingManager;
    public Rigidbody rb;
    public NavMeshAgent agent;
    public Animator animator;
    public GameObject hook;
    public HingeJoint hookHinge;

    public bool thisFishToHook;

    //Fish movement
    [SerializeField] public float swimPointRange = 3f;
    [SerializeField] public float sightRange = 3f;
    [SerializeField] public float waitTimeToDie = 4f;

    public Vector3 swimPoint;
    public bool swimPointSet = false;

    //Fish biting variables
    [SerializeField] public int minBites = 2, maxBites = 5;
    [SerializeField] public float distanceToMoveWhenBiting = 2.5f;
    [SerializeField] public float bitingSpeed = 1f;
    [SerializeField] public float waitTimeToCatch = 3f;

    // Start is called before the first frame update
    void Start()
    {
        //Get Components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); 
        rb = GetComponent<Rigidbody>();
        fishingManager = GameObject.FindGameObjectWithTag("FishingManager").GetComponent<FishingManager>();
        hook = fishingManager.hook;
        hookHinge = hook.GetComponent<HingeJoint>();

        //Enter initial state
        currentState = patrollingState;

        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(FishBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void FindSwimPoint()
    {
        float randomZ = Random.Range(-swimPointRange, swimPointRange);
        float randomX = Random.Range(-swimPointRange, swimPointRange);

        swimPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (NavMesh.SamplePosition(swimPoint, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            swimPointSet = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}
