using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningState : PlayerState {
    [SerializeField] string animationName = "IsRunning";

    public override void EnterState(PlayerStateManager stateManager, PlayerBase player) {
        player.animator.SetBool(animationName, true);
    }

    public override void ExitState(PlayerStateManager stateManager, PlayerBase player) {
        player.animator.SetBool(animationName, false);
    }

    public override void UpdateState(PlayerStateManager stateManager, PlayerBase player) {
    }
}