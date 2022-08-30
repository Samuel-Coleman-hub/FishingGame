using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FishPatrollingState : FishBaseState
{
    private bool hookInSightRange;

    private FishStateManager fish;
    private FishingManager fishingManager;

    private NavMeshAgent agent;
    private Animator animator;

    public override void EnterState(FishStateManager fish)
    {
        this.fish = fish;
        agent = this.fish.agent;
        animator = this.fish.animator;
        animator.SetTrigger("Swim");
        fishingManager = fish.fishingManager;

    }

    public override void UpdateState(FishStateManager fish)
    {
        hookInSightRange = Physics.CheckSphere(fish.transform.position, fish.sightRange, fish.whatIsPlayer);

        if (!hookInSightRange)
        {
            Patrolling(fish);
        }
        else if(fishingManager.fishingRodCast && (!fishingManager.fishAtHook || fish.thisFishToHook))
        {
            Debug.Log("Hook is in sight switching state" + "is fishing rod casting: " + fishingManager.fishingRodCast);
            Debug.Log("is fish at hook + " + fishingManager.fishAtHook);
            Debug.Log("is this fish at hook " + fish.thisFishToHook);
            Debug.Log("is hook in sight range " + hookInSightRange);
            fish.SwitchState(fish.swimToHookState);
        }
        
    }

    private void Patrolling(FishStateManager fish)
    {
        if (!fish.swimPointSet)
        {
            fish.FindSwimPoint();
        }
        else
        {
            agent.SetDestination(fish.swimPoint);
        }

        Vector3 distanceToSwimPoint = fish.transform.position - fish.swimPoint;

        if (distanceToSwimPoint.magnitude < 1f)
        {
            fish.swimPointSet = false;
        }
    }
}
