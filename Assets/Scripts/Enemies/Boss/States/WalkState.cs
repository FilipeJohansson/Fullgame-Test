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

        if(enemyBase.canAttack)
            boss.SwitchState(boss.AttackState);
        else 
            enemyBase.DecreaseAttackTimer();

        if (enemyBase.attackTimer <= 0)
            enemyBase.canAttack = true;
    }
}