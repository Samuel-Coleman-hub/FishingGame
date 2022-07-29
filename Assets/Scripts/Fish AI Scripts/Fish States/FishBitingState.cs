using UnityEngine;

public class FishBitingState : FishBaseState
{
    private Vector3[] bitingPositions = new Vector3[2];

    private Transform hook;
    private int bitingState = 0;
    private int biteCounter = 0;
    private int totalBites;

    public override void EnterState(FishStateManager fish)
    {
        hook = fish.fishingManager.hook.transform;

        StartBitingSequence(fish);
    }

    public override void UpdateState(FishStateManager fish)
    {
        BitingHook(fish);
    }

    private void StartBitingSequence(FishStateManager fish)
    {
        fish.fishingManager.BobHook();
        Vector3 behindFish = fish.transform.position - (fish.transform.forward * fish.distanceToMoveWhenBiting);
        Vector3 hookPos = new Vector3(hook.position.x, hook.position.y, hook.position.z);

        biteCounter = 0;
        totalBites = Random.Range(fish.minBites, fish.maxBites);

        bitingPositions[0] = behindFish;
        bitingPositions[1] = hookPos;
        fish.agent.enabled = false;
    }

    private void BitingHook(FishStateManager fish)
    {
        if (Vector3.Distance(bitingPositions[bitingState], fish.transform.position) < 1f)
        {
            if (bitingState == 0)
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
                    fish.fishingManager.BobHook();
                }
                else
                {
                    fish.SwitchState(fish.hookedState);
                }
            }
        }

        fish.transform.position = Vector3.MoveTowards(fish.transform.position, bitingPositions[bitingState], Time.deltaTime * fish.bitingSpeed);
    }
}
