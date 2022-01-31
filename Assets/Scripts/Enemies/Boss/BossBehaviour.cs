using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : BossBase {
    void Awake() {
        attacks = new List<IAttack> {
            new SimpleAttack(gameObject),
            new DashAttack(gameObject)
        };
    }
}
