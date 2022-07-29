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
    public NavMeshAgent agent;
    public Animator animator;

    public bool thisFishToHook;

    //Fish movement
    [SerializeField] public float swimPointRange = 3;
    [SerializeField] public float sightRange = 3;

    //Fish biting variables
    [SerializeField] public int minBites = 2, maxBites = 5;
    [SerializeField] public float distanceToMoveWhenBiting = 2.5f;
    [SerializeField] public float bitingSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //Get Components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fishingManager = GameObject.FindGameObjectWithTag("FishingManager").GetComponent<FishingManager>();



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
}
