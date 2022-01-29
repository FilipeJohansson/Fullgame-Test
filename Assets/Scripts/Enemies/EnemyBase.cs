using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {

    // [SerializeField] public GameManager gameManager;
    [SerializeField] public Rigidbody2D rb;
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
    [SerializeField] public Transform attackPoint;
    [SerializeField] public float attackRange = 1f;

    bool isFlipped = false;

    public IAttack activeAttack;
    public List<IAttack> attacks;

    [Header("Context Attributes")]
    [SerializeField] protected GameManager gameManager;

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
        attackTimer = attackCooldown;
        attackDelayTimer = attackDelay;

        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        LookAtPlayer();
        FollowPlayer();
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

    public void Attack() {
        activeAttack = attacks[Random.Range(0, attacks.Count)];
        activeAttack.Attack(this);
    }

    public void TakeDamage(float amount) {
        currentHealth -= (int)amount;
        if (currentHealth <= 0) {
            // Die();
        }
    }

    public void LookAtPlayer() {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1;

        if (transform.position.x > GameManager.Player.transform.position.x && isFlipped) {
            transform.localScale = flipped;
            transform.Rotate(0, 180, 0);
            isFlipped = false;
        } else if (transform.position.x < GameManager.Player.transform.position.x && !isFlipped) {
            transform.localScale = flipped;
            transform.Rotate(0, 180, 0);
            isFlipped = true;

        }
    }

    public void FollowPlayer() {
        // transform.position = Vector3.MoveTowards(transform.position, GameManager.Player.transform.position, speed * Time.deltaTime);
        Vector2 target = new Vector2(GameManager.Player.transform.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.deltaTime);
        rb.MovePosition(newPos);
    }

    // public void Die() {
    //     gameManager.EnemyDied(this);
    //     Destroy(gameObject);
    // }

    // void OnDrawGizmosSelected() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    // }
}
