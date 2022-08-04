using UnityEngine;

public abstract class FishBaseState
{
    public abstract void EnterState(FishStateManager fish);

    public abstract void UpdateState(FishStateManager fish);
}