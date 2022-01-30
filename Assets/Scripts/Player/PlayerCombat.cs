using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    [SerializeField] public int attackDamage = 3;
    [SerializeField] private Transform m_AttackCheck;
    [SerializeField] private float k_AttackRadius;
    [SerializeField] private LayerMask m_WhatIsEnemy;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Awake() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Attack"))
            animator.SetTrigger("IsAttacking");
    }

    public void Attack() {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(m_AttackCheck.position, k_AttackRadius, m_WhatIsEnemy);

        foreach (Collider2D enemy in enemies)
            enemy.GetComponent<EnemyBase>().TakeDamage(attackDamage);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_AttackCheck.position, k_AttackRadius);
    }
}
