using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningState : PlayerState {
    [SerializeField] string animationName = "Speed";

    public override void EnterState(PlayerStateManager stateManager, PlayerBase player) {
        // boss.animator.SetBool(animationName, true);
        stateManager.animator.SetBool(animationName, true);
    }

    public override void ExitState(PlayerStateManager stateManager, PlayerBase player) {
        stateManager.animator.SetBool(animationName, false);
    }

    public override void UpdateState(PlayerStateManager stateManager, PlayerBase player) {
    }
}