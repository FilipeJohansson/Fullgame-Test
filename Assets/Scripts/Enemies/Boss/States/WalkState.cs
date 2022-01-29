using System.Collections;
using UnityEngine;

public class WalkState : BossBaseState {
    [SerializeField] string animationName = "walking";

    public override void EnterState(BossStateManager boss, BossBase bossBase) {
        boss.animator.SetBool(animationName, true);
        boss.StartCoroutine(Attack(boss));
    }

    public override void ExitState(BossStateManager boss, BossBase bossBase) {
        boss.animator.SetBool(animationName, false);
    }

    IEnumerator Attack(BossStateManager boss) {
        yield return new WaitForSeconds(2f);

        boss.SwitchState(boss.AttackState);

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
    }
}