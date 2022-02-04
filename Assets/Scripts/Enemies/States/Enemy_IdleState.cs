using System.Collections;
using UnityEngine;

public class Enemy_IdleState : EnemyState {
    [SerializeField] string animationName = "idlening";

    public override void EnterState(EnemyStateManager enemyStateManager, EnemyBase enemyBase) {
        enemyStateManager.animator.SetBool(animationName, true);
    }

    public override void ExitState(EnemyStateManager enemyStateManager, EnemyBase enemyBase) {
        enemyStateManager.animator.SetBool(animationName, false);
    }

    public override void UpdateState(EnemyStateManager enemyStateManager, EnemyBase enemyBase) {
        if (enemyStateManager.enemyBase.gameManager.runningGame)
            enemyStateManager.SwitchState(enemyStateManager.WalkState);
    }
}