using System.Collections;
using UnityEngine;

public class DashState : BossBaseState {
    [SerializeField] string animationName = "dashing";

    public override void EnterState(BossStateManager boss, EnemyBase enemyBase) {
        boss.animator.SetBool(animationName, true);

        enemyBase.isAttacking = true;
    }

    public override void ExitState(BossStateManager boss, EnemyBase enemyBase) {
        boss.animator.SetBool(animationName, false);

        enemyBase.canAttack = false;
        enemyBase.ResetAttackTimer();
    }

    public override void UpdateState(BossStateManager boss, EnemyBase enemyBase) {
        if (!enemyBase.isAttacking)
            boss.SwitchState(boss.WalkState);
    }
}