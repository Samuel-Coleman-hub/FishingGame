using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishStateManager : MonoBehaviour
{
    //References the active state in the state machine
    FishBaseState currentState;

    //State References
    public FishPatrollingState patrollingState = new FishPatrollingState();
    public FishSwimToHookState swimToHookState = new FishSwimToHookState();
    public FishBitingState bitingState = new FishBitingState();
    public FishHookedState hookedState = new FishHookedState();
    public FishReelState reelState = new FishReelState();
    public FishEscapeState escapeState = new FishEscapeState();

    //StateMachine Settings
    [Header("State Machine Settings")]
    [SerializeField] public LayerMask whatIsSeaBed;
    [SerializeField] public LayerMask whatIsPlayer;
    [HideInInspector] public bool thisFishToHook;
    [HideInInspector] public Vector3 swimPoint;
    [HideInInspector] public bool swimPointSet = false;

    [Header("Unique Fish Settings")]
    [SerializeField] public GameObject fishPrefab;
    [SerializeField] public FishTracker.Fishes typeOfFish = FishTracker.Fishes.Seabass;
    //Fish movement
    [Header("Fish Movement Settings")]
    [SerializeField] public float swimPointRange = 3f;
    [SerializeField] public float sightRange = 3f;
    [SerializeField] public float waitTimeToDie = 3f;
    //Fish biting variables
    [Header("Fish Biting Settings")]
    [SerializeField] public int minBites = 2, maxBites = 5;
    [SerializeField] public float distanceToMoveWhenBiting = 2.5f;
    [SerializeField] public float bitingSpeed = 1f;
    [SerializeField] public float waitTimeToCatch = 3f;

    //Fish Component Variables
    [HideInInspector] public FishingManager fishingManager;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public GameObject hook;
    [HideInInspector] public HingeJoint hookHinge;

    public (FishTracker.Fishes, GameObject) fishData;

    // Start is called before the first frame update
    void Start()
    {
        //Tuple of data to be passed when caught
        fishData = (typeOfFish, fishPrefab);

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
