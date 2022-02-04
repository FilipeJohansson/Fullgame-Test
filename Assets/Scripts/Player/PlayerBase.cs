using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour {
    [SerializeField] public Rigidbody2D rb;
    public CharacterController2D controller;
    public Animator animator;
    public SlideBar healthBar;
    public SlideBar staminaBar;

    [SerializeField] public int maxHealth;
    [SerializeField] public int currentHealth;
    [SerializeField] public int maxStamina;
    [SerializeField] public int currentStamina;
    [SerializeField] public bool isAttacking = false;
    [SerializeField] public bool isJumpAttacking = false;

    [SerializeField] public int attackDamage = 3;
    [SerializeField] private Transform m_AttackCheck;
    [SerializeField] private float k_AttackRadius;
    [SerializeField] private Transform m_JumpAttackCheck;
    [SerializeField] private float k_JumpAttackSizeX;
    [SerializeField] private float k_JumpAttackSizeY;
    [SerializeField] private LayerMask m_WhatIsEnemy;

    [SerializeField] public float horizontalMove;

    [SerializeField] private SimpleFlash simpleFlash;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public GameObject stunObject;

    [SerializeField] public float untargetableCooldown = 0.5f;
    [SerializeField] public float untargetableTimer = 0;

    [SerializeField] public float attackCooldown = 0.5f;
    [SerializeField] public float attackTimer = 0;

    [SerializeField] public float dashCooldown = 0.5f;
    [SerializeField] public float dashTimer = 0;

    [SerializeField] public float refreshStaminaCooldown = 2f;
    [SerializeField] public float refreshStaminaTimer = 0;

    [SerializeField] public Animator camAnimator;

    public bool jump = false;
    public bool isDead;
    public bool isInTheAir = false;
    public bool isStuned = false;
    public bool isDashing = false;
    public bool isUntargetable = false;

    [Header("Context Attributes")]
    [SerializeField] protected GameManager gameManager;

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        refreshStaminaTimer = refreshStaminaCooldown;
        untargetableTimer = untargetableCooldown;

        healthBar.SetMaxValue(maxHealth);
        staminaBar.SetMaxValue(maxStamina);

        simpleFlash = gameObject.GetComponent<SimpleFlash>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {
        if (isDead || !gameManager.runningGame)
            return;

        handleMovement();
        HandleAttack();
        RefreshStamina();

        if (isUntargetable && untargetableTimer > 0)
            untargetableTimer -= Time.deltaTime;
        else if (isUntargetable && untargetableTimer <= 0) {
            isUntargetable = false;
            untargetableTimer = untargetableCooldown;
        }

        staminaBar.SetValue(currentStamina);
    }

    void FixedUpdate() {
        if (isDead || !gameManager.runningGame)
            return;

        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    private void HandleAttack() {
        if (isStuned)
            return;

        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Attack") && attackTimer <= 0) {
            attackTimer = attackCooldown;

            if (isInTheAir) {
                if (currentStamina > 0) {
                    isJumpAttacking = true;

                    currentStamina--;
                    staminaBar.SetValue(currentStamina);
                }
            } else
                isAttacking = true;
        }
    }

    private void RefreshStamina() {
        if (currentStamina == maxStamina)
            return;

        if (refreshStaminaTimer > 0)
            refreshStaminaTimer -= Time.deltaTime;

        if (refreshStaminaTimer <= 0) {
            currentStamina++;
            refreshStaminaTimer = refreshStaminaCooldown;
        }
    }

    // Private methods
    private void handleMovement() {
        if (isStuned)
            return;

        if (dashTimer > 0)
            dashTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Dash") && dashTimer <= 0) {
            if (currentStamina > 0) {
                dashTimer = dashCooldown;

                currentStamina--;
                staminaBar.SetValue(currentStamina);
                StartCoroutine(DashCoroutine(0.1f));
            }
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * controller.runSpeed;

        if (Input.GetButtonDown("Jump")) {
            jump = true;
            isInTheAir = true;
        }
    }

    // Public methods
    public void onLanding() {
        isInTheAir = false;
        isAttacking = false;
        isJumpAttacking = false;
    }

    public void TakeDamage(int amount) {
        if (isUntargetable)
            return;

        camAnimator.SetTrigger("Shake");

        isUntargetable = true;
        untargetableTimer = untargetableCooldown;

        currentHealth -= amount;
        healthBar.SetValue(currentHealth);

        simpleFlash.Flash();

        if (currentHealth <= 0)
            Die();
    }

    public void Stun(float duration) {
        if (isUntargetable)
            return;

        horizontalMove = 0;
        isStuned = true;
        stunObject.SetActive(true);
        StartCoroutine(StunCoroutine(duration));
    }

    public void Attack() {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(m_AttackCheck.position, k_AttackRadius, m_WhatIsEnemy);

        foreach (Collider2D enemy in enemies)
            enemy.GetComponent<EnemyBase>().TakeDamage(attackDamage);
    }

    public void JumpAttack() {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(m_JumpAttackCheck.position, new Vector2(k_JumpAttackSizeX, k_JumpAttackSizeY), 0, m_WhatIsEnemy);

        foreach (Collider2D enemy in enemies)
            enemy.GetComponent<EnemyBase>().TakeDamage(attackDamage);

        controller.ApplyNegativeForce();
    }

    public void ResetAttack() {
        isAttacking = false;
        isJumpAttacking = false;
    }

    public void Die() {
        isDead = true;
        rb.velocity = Vector2.zero;
        gameManager.runningGame = false;
        StartCoroutine(DeathAnimation(100));
    }

    IEnumerator DeathAnimation(float duration) {
        Color deathColor = spriteRenderer.color;
        deathColor.a = 0;

        for (float t = 0f; t < duration; t += Time.deltaTime) {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, deathColor, normalizedTime);
            yield return null;
        }
        spriteRenderer.color = deathColor; //without this, the value will end at something like 0.9992367

        gameManager.PauseGame();
    }

    IEnumerator StunCoroutine(float duration) {
        yield return new WaitForSeconds(duration);
        isStuned = false;
        stunObject.SetActive(false);
        yield return null;
    }

    IEnumerator DashCoroutine(float duration) {
        isDashing = true;
        isUntargetable = true;
        float oldGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.AddForce(new Vector2(2 * horizontalMove, 0), ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        rb.gravityScale = 3f;
        isDashing = false;

        yield return null;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_AttackCheck.position, k_AttackRadius);

        Gizmos.DrawWireCube(m_JumpAttackCheck.position, new Vector3(k_JumpAttackSizeX, k_JumpAttackSizeY, 0));
    }
}
