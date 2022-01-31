using System.Collections;
using UnityEngine;

public class IdleState : BossBaseState {
    [SerializeField] string animationName = "idlening";

    public override void EnterState(BossStateManager boss, EnemyBase enemyBase) {
        boss.animator.SetBool(animationName, true);
    }

    public override void ExitState(BossStateManager boss, EnemyBase enemyBase) {
        boss.animator.SetBool(animationName, false);
    }

    public override void UpdateState(BossStateManager boss, EnemyBase enemyBase) {
        if (boss.bossBase.gameManager.runningGame)
            boss.SwitchState(boss.WalkState);
    }
}