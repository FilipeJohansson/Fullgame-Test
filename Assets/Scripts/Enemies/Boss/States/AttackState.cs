using System.Collections;
using UnityEngine;

public class AttackState : BossBaseState {
    [SerializeField] string animationName = "attacking";

    public override void EnterState(BossStateManager boss, BossBase bossBase) {
        boss.animator.SetBool(animationName, true);
        Attack(bossBase);
        boss.StartCoroutine(Walk(boss));
    }

    public override void ExitState(BossStateManager boss, BossBase bossBase) {
        boss.animator.SetBool(animationName, false);
    }

    IEnumerator Walk(BossStateManager boss) {
        yield return new WaitForSeconds(2f);

        boss.SwitchState(boss.WalkState);

        yield return null;
    }

    public override void UpdateState(BossStateManager boss, BossBase bossBase) {
        // bossBase.LookAtPlayer();
        // boss.transform.position = Vector3.MoveTowards(boss.transform.position, bossBase.player.transform.position, bossBase.speed * Time.deltaTime);

        // // Verify if can attack
        // if (bossBase.canAttack) {
        //     bossBase.ResetAttackTimer();
        //     boss.SwitchState(boss.AttackState);
        // } else {
        //     bossBase.DecreaseAttackTimer();

        //     if (bossBase.attackTimer <= 0)
        //         bossBase.canAttack = true;
        // }

        // if (bossBase.attackDelayTimer <= 0) {


        //     bossBase.ResetAttackDelay();
        // }

        // if (!bossBase.canAttack)
        //     boss.SwitchState(boss.WalkState);
    }

    public void Attack(BossBase bossBase) {
        
    }
}