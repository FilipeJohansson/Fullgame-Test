using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {

    // [SerializeField] public GameManager gameManager;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public int maxHealth;
    [SerializeField] public int currentHealth;
    [SerializeField] public float damage;
    [SerializeField] public int collisionDamage;
    [SerializeField] public float speed;
    [SerializeField] public float attackCooldown = 3f;
    [SerializeField] public float attackTimer;
    [SerializeField] public bool canAttack = false;
    [SerializeField] public bool isAttacking = false;
    [SerializeField] public Transform attackPoint;
    [SerializeField] public float attackRange = 1f;
    [SerializeField] public bool inAttackRange = false;
    [SerializeField] public Transform bodyDamagePoint;
    [SerializeField] public float bodyDamageX = 1f;
    [SerializeField] public float bodyDamageY = 1f;
    [SerializeField] public LayerMask whatIsPlayer;

    public bool m_FacingRight = true;

    [SerializeField] private SimpleFlash spriteRendererFlash;

    public IAttack activeAttack;
    public List<IAttack> attacks;

    [Header("Context Attributes")]
    [SerializeField] public GameManager gameManager;

    // Start is called before the first frame update
    public virtual void Start() {
        currentHealth = maxHealth;
        ResetAttackTimer();

        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        spriteRendererFlash = gameObject.GetComponentInChildren<SimpleFlash>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!gameManager.runningGame)
            return;

        VerifyRangeAttack();

        Collider2D[] player = Physics2D.OverlapBoxAll(bodyDamagePoint.position, new Vector2(bodyDamageX, bodyDamageY), 0, whatIsPlayer);
        if (player.Length > 0)
            GameManager.Player.gameObject.GetComponent<PlayerBase>().TakeDamage(collisionDamage);
    }

    public void ResetAttackTimer() {
        attackTimer = attackCooldown;
    }

    public void DecreaseAttackTimer() {
        attackTimer -= Time.deltaTime;
    }

    public void EndAttack() {
        isAttacking = false;
    }

    public void TakeDamage(float amount) {
        currentHealth -= (int)amount;

        spriteRendererFlash.Flash();

        if (currentHealth <= 0)
            Die();
    }

    public void LookAtPlayer() {

        if (transform.position.x > GameManager.Player.transform.position.x && !m_FacingRight)
            Flip();
        else if (transform.position.x < GameManager.Player.transform.position.x && m_FacingRight)
            Flip();
    }

    private void Flip() {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void FollowPlayer() {
        Vector2 target = new Vector2(GameManager.Player.transform.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.deltaTime);
        rb.MovePosition(newPos);
    }

    public void VerifyRangeAttack() {
        Collider2D[] player = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, whatIsPlayer);
        if (player.Length > 0)
            inAttackRange = true;
        else
            inAttackRange = false;
    }

    public virtual void Die() {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireCube(bodyDamagePoint.position, new Vector3(bodyDamageX, bodyDamageY, 0));
    }
}
