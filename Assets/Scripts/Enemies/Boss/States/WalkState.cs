using System.Collections;
using UnityEngine;

public class WalkState : BossBaseState {
    [SerializeField] string animationName = "walking";

    public override void EnterState(BossStateManager boss, EnemyBase enemyBase) {
        boss.animator.SetBool(animationName, true);
    }

    public override void ExitState(BossStateManager boss, EnemyBase enemyBase) {
        boss.animator.SetBool(animationName, false);
    }

    public override void UpdateState(BossStateManager boss, EnemyBase enemyBase) {
        enemyBase.LookAtPlayer();
        if (!enemyBase.inAttackRange)
            enemyBase.FollowPlayer();

        if(enemyBase.canAttack && enemyBase.inAttackRange)
            boss.SwitchState(boss.AttackState);
        else 
            enemyBase.DecreaseAttackTimer();

        if (enemyBase.attackTimer <= 0)
            enemyBase.canAttack = true;
    }
}