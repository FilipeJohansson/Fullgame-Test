using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {

    // [SerializeField] public GameManager gameManager;
    [SerializeField] public int maxHealth;
    [SerializeField] public int currentHealth;
    [SerializeField] public float damage;
    [SerializeField] public float speed;
    [SerializeField] public float attackCooldown = 3f;
    [SerializeField] public float attackTimer;
    [SerializeField] public float attackDelay = 3f;
    [SerializeField] public float attackDelayTimer;
    [SerializeField] public bool canAttack = false;
    [SerializeField] public bool isAttacking = false;

    public IAttack activeAttack;
    public List<IAttack> attacks;

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
        attackTimer = attackCooldown;
        attackDelayTimer = attackDelay;
    }

    // Update is called once per frame
    void Update() {

    }

    public void ResetAttackTimer() {
        attackTimer = attackCooldown;
    }

    public void DecreaseAttackTimer() {
        attackTimer -= Time.deltaTime;
    }

    public void ResetAttackDelay() {
        attackDelayTimer = attackDelay;
    }

    public void DecreaseAttackDelay() {
        attackTimer -= Time.deltaTime;
    }

    public void TakeDamage(float amount) {
        currentHealth -= (int)amount;
        if (currentHealth <= 0) {
            // Die();
        }
    }

    // public void Die() {
    //     gameManager.EnemyDied(this);
    //     Destroy(gameObject);
    // }
}
