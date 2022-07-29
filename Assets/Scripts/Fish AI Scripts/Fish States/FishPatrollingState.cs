using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FishPatrollingState : FishBaseState
{
    private Vector3 swimPoint;
    private bool swimPointSet = false;

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
            fish.SwitchState(fish.swimToHookState);
        }
        
    }

    private void Patrolling(FishStateManager fish)
    {
        if (!swimPointSet)
        {
            FindSwimPoint(fish);
        }
        else
        {
            agent.SetDestination(swimPoint);
        }

        Vector3 distanceToSwimPoint = fish.transform.position - swimPoint;

        if (distanceToSwimPoint.magnitude < 1f)
        {
            swimPointSet = false;
        }
    }

    private void FindSwimPoint(FishStateManager fish)
    {
        float randomZ = Random.Range(-fish.swimPointRange, fish.swimPointRange);
        float randomX = Random.Range(-fish.swimPointRange, fish.swimPointRange);

        swimPoint = new Vector3(fish.transform.position.x + randomX, fish.transform.position.y, fish.transform.position.z + randomZ);

        if (NavMesh.SamplePosition(swimPoint, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            swimPointSet = true;
        }
    }
}
