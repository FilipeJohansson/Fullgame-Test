using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour, IAttack {
    protected GameObject owner;
    public int attackDamage = 10;

    public DashAttack(GameObject _owner) {
        owner = _owner;
    }

    public void Attack(MonoBehaviour mono) {
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "PlayerDamage") {
            PlayerBase player = other.GetComponentInParent<PlayerBase>();
            player.Stun(1f);
            player.TakeDamage(attackDamage);
        }
    }
}
