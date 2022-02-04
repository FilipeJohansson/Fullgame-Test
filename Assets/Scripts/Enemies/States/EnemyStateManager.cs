using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour {
    [SerializeField] EnemyState currentState;

    public Enemy_WalkState WalkState = new Enemy_WalkState();
    public Enemy_AttackState AttackState = new Enemy_AttackState();
    public Enemy_IdleState IdleState = new Enemy_IdleState();

    public Animator animator;
    public EnemyBase enemyBase;

    void Awake() {
        animator = gameObject.GetComponent<Animator>();
        enemyBase = gameObject.GetComponent<EnemyBase>();
    }

    // Start is called before the first frame update
    void Start() {
        currentState = WalkState;
        currentState.EnterState(this, enemyBase);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!enemyBase.gameManager.runningGame) {
            if (currentState != IdleState)
                SwitchState(IdleState);
            return;
        }
        currentState.UpdateState(this, enemyBase);
    }

    public void SwitchState(EnemyState state) {
        currentState.ExitState(this, enemyBase);
        currentState = state;
        state.EnterState(this, enemyBase);
    }
}