using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState {
    [SerializeField] string animationName = "IsJumping";

    public override void EnterState(PlayerStateManager stateManager, PlayerBase player) {
        stateManager.animator.SetBool(animationName, true);
    }

    public override void ExitState(PlayerStateManager stateManager, PlayerBase player) {
        stateManager.animator.SetBool(animationName, false);
    }

    public override void UpdateState(PlayerStateManager stateManager, PlayerBase player) {
        if (player.isJumpAttacking)
            stateManager.SwitchState(stateManager.PlayerJumpAttackState);

        if (!player.isInTheAir) {
            if (player.horizontalMove != 0)
                stateManager.SwitchState(stateManager.RunningState);
            else
                stateManager.SwitchState(stateManager.PlayerIdleState);
        }
    }
}