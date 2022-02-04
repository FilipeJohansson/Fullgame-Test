using System.Collections;
using UnityEngine;

public class Enemy_WalkState : EnemyState {
    [SerializeField] string animationName = "walking";

    public override void EnterState(EnemyStateManager enemyStateManager, EnemyBase enemyBase) {
        enemyStateManager.animator.SetBool(animationName, true);
    }

    public override void ExitState(EnemyStateManager enemyStateManager, EnemyBase enemyBase) {
        enemyStateManager.animator.SetBool(animationName, false);
    }

    public override void UpdateState(EnemyStateManager enemyStateManager, EnemyBase enemyBase) {
        enemyBase.LookAtPlayer();
        enemyBase.FollowPlayer();

        if (enemyBase.canAttack && enemyBase.inAttackRange)
            enemyStateManager.SwitchState(enemyStateManager.AttackState);
        // if (enemyBase.canAttack && !enemyBase.inAttackRange)
        // enemyStateManager.SwitchState(enemyStateManager.DashState);
        else
            enemyBase.DecreaseAttackTimer();

        if (enemyBase.attackTimer <= 0)
            enemyBase.canAttack = true;
    }
}