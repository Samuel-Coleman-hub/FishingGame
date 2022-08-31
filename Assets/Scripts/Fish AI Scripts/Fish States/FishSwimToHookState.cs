using UnityEngine;

public class FishSwimToHookState : FishBaseState
{
    private FishingManager fishingManager;
    private Transform hookPos;

    public override void EnterState(FishStateManager fish)
    {
        fishingManager = fish.fishingManager;
        hookPos = fishingManager.hook.transform;
    }

    public override void UpdateState(FishStateManager fish)
    {
        if (fishingManager.fishingRodCast)
        {
            SwimToHook(fish);
        }
        else
        {
            fish.SwitchState(fish.patrollingState);
        }
    }

    private void SwimToHook(FishStateManager fish)
    {
        fish.fishingManager.fishAtHook = true;
        fish.thisFishToHook = true;
        fish.agent.SetDestination(hookPos.position);

        Vector3 distanceToHook = fish.transform.position - hookPos.position;

        if (distanceToHook.magnitude < 1f)
        {
            fish.SwitchState(fish.bitingState);
        }
    }
}
