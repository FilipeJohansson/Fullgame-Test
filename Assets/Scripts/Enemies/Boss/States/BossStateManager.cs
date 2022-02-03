using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateManager : MonoBehaviour {
    [SerializeField] BossBaseState currentState;

    public WalkState WalkState = new WalkState();
    public AttackState AttackState = new AttackState();
    public DashState DashState = new DashState();
    public IdleState IdleState = new IdleState();

    public Animator animator;
    public BossBase bossBase;

    void Awake() {
        animator = gameObject.GetComponent<Animator>();
        bossBase = gameObject.GetComponent<BossBase>();
    }

    // Start is called before the first frame update
    void Start() {
        currentState = WalkState;
        currentState.EnterState(this, bossBase);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!bossBase.gameManager.runningGame) {
            if (currentState != IdleState)
                SwitchState(IdleState);
            return;
        }
        currentState.UpdateState(this, bossBase);
    }

    public void SwitchState(BossBaseState state) {
        currentState.ExitState(this, bossBase);
        currentState = state;
        state.EnterState(this, bossBase);
    }
}