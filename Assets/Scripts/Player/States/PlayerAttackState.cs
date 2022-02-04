using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState {
    [SerializeField] string animationName = "IsAttacking";

    public override void EnterState(PlayerStateManager stateManager, PlayerBase player) {
        stateManager.animator.SetTrigger(animationName);
    }

    public override void ExitState(PlayerStateManager stateManager, PlayerBase player) {
        stateManager.animator.ResetTrigger(animationName);
    }

    public override void UpdateState(PlayerStateManager stateManager, PlayerBase player) {
        if (player.isJumpAttacking)
            return;

        if (!player.isAttacking)
            if (player.horizontalMove != 0 && !player.isInTheAir)
                stateManager.SwitchState(stateManager.RunningState);
            else if (player.isJumping)
                stateManager.SwitchState(stateManager.PlayerJumpState);
            else
                stateManager.SwitchState(stateManager.PlayerIdleState);
    }
}