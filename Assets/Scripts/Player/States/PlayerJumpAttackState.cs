using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpAttackState : PlayerState {
    [SerializeField] string animationName = "IsJumpAttacking";

    public override void EnterState(PlayerStateManager stateManager, PlayerBase player) {
        stateManager.animator.SetTrigger(animationName);
        player.isUntargetable = true;
    }

    public override void ExitState(PlayerStateManager stateManager, PlayerBase player) {
        stateManager.animator.ResetTrigger(animationName);
        player.untargetableTimer = player.untargetableCooldown;
    }

    public override void UpdateState(PlayerStateManager stateManager, PlayerBase player) {
        if (!player.isJumpAttacking)
            stateManager.SwitchState(stateManager.PlayerJumpState);
    }
}