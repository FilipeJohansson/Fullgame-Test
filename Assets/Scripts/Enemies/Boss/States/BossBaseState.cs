using UnityEngine;

public abstract class BossBaseState {
    public abstract void EnterState(BossStateManager boss, EnemyBase enemyBase);
    public abstract void ExitState(BossStateManager boss, EnemyBase enemyBase);
    public abstract void UpdateState(BossStateManager boss, EnemyBase enemyBase);
}