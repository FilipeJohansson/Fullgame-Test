using UnityEngine;

public abstract class EnemyState {
    public abstract void EnterState(EnemyStateManager enemyStateManager, EnemyBase enemyBase);
    public abstract void ExitState(EnemyStateManager enemyStateManager, EnemyBase enemyBase);
    public abstract void UpdateState(EnemyStateManager enemyStateManager, EnemyBase enemyBase);
}