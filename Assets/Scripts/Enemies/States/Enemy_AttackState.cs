using System.Collections;
using UnityEngine;

public class Enemy_AttackState : EnemyState {
    [SerializeField] string animationName = "attacking";

    public override void EnterState(EnemyStateManager enemyStateManager, EnemyBase enemyBase) {
        enemyStateManager.animator.SetBool(animationName, true);

        enemyBase.isAttacking = true;
    }

    public override void ExitState(EnemyStateManager enemyStateManager, EnemyBase enemyBase) {
        enemyStateManager.animator.SetBool(animationName, false);

        enemyBase.canAttack = false;
        enemyBase.ResetAttackTimer();
    }

    public override void UpdateState(EnemyStateManager enemyStateManager, EnemyBase enemyBase) {
        if (!enemyBase.isAttacking)
            enemyStateManager.SwitchState(enemyStateManager.WalkState);
    }
}