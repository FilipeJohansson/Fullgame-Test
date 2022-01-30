using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour, IAttack {

    GameObject owner;

    public DashAttack(GameObject owner) {
        owner = owner;
    }

    public void Attack(MonoBehaviour mono) {
        owner.GetComponent<BossBase>().rb.AddForce(new Vector2(20, 0), ForceMode2D.Impulse);
    }
}
