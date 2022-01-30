using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour {
    [SerializeField] PlayerState currentState;

    // States
    public PlayerIdleState PlayerIdleState = new PlayerIdleState();
    public PlayerRunningState RunningState = new PlayerRunningState();
    public PlayerAttackState PlayerAttackState = new PlayerAttackState();
    public PlayerJumpAttackState PlayerJumpAttackState = new PlayerJumpAttackState();

    public Animator animator;
    public PlayerBase playerBase;
    
    // Start is called before the first frame update
    void Start() {
        currentState = PlayerIdleState;
        currentState.EnterState(this, playerBase);
    }

    // Update is called once per frame
    void Update() {
        currentState.UpdateState(this, playerBase);
    }

    public void SwitchState(PlayerState state) {
        currentState.ExitState(this, playerBase);
        currentState = state;
        state.EnterState(this, playerBase);
    }
}
