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
        if (player.horizontalMove != 0)
            stateManager.SwitchState(stateManager.RunningState);

        // if (Input.GetButtonDown("Jump")) {
        //     //player.animator.SetBool("IsJumping", true);
        //     player.jump = true;
        // }
    }
}