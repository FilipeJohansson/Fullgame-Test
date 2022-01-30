using System.Collections;
using UnityEngine;

public class AttackState : BossBaseState {
    [SerializeField] string animationName = "attacking";

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
        // bossBase.LookAtPlayer();
        // boss.transform.position = Vector3.MoveTowards(boss.transform.position, bossBase.player.transform.position, bossBase.speed * Time.deltaTime);

        // // Verify if can attack
        // if (bossBase.canAttack) {
        //     bossBase.ResetAttackTimer();
        //     boss.SwitchState(boss.AttackState);
        // } else {
        //     bossBase.DecreaseAttackTimer();

        //     if (bossBase.attackTimer <= 0)
        //         bossBase.canAttack = true;
        // }

        // if (bossBase.attackDelayTimer <= 0) {


        //     bossBase.ResetAttackDelay();
        // }

        if (!enemyBase.isAttacking)
            boss.SwitchState(boss.WalkState);
    }
}