using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState {
    [SerializeField] string animationName = "IsAttacking";

    public override void EnterState(PlayerStateManager stateManager, PlayerBase player) {
    }

    public override void ExitState(PlayerStateManager stateManager, PlayerBase player) {
    }

    public override void UpdateState(PlayerStateManager stateManager, PlayerBase player) {
    }
}