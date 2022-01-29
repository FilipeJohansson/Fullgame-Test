using UnityEngine;

public abstract class BossBaseState {
    public abstract void EnterState(BossStateManager boss, BossBase bossBase);
    public abstract void ExitState(BossStateManager boss, BossBase bossBase);
    public abstract void UpdateState(BossStateManager boss, BossBase bossBase);
}