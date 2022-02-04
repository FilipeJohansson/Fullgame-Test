using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState {
    [SerializeField] string animationName = "IsIdle";

    public override void EnterState(PlayerStateManager stateManager, PlayerBase player) {
        stateManager.animator.SetBool(animationName, true);
    }

    public override void ExitState(PlayerStateManager stateManager, PlayerBase player) {
        stateManager.animator.SetBool(animationName, false);
    }

    public override void UpdateState(PlayerStateManager stateManager, PlayerBase player) {
        if (player.horizontalMove != 0 && !player.isInTheAir)
            stateManager.SwitchState(stateManager.RunningState);

        if (player.isJumping)
            stateManager.SwitchState(stateManager.PlayerJumpState);

        if (player.groundAttackAttributes.isAttacking)
            stateManager.SwitchState(stateManager.PlayerAttackState);
    }
}