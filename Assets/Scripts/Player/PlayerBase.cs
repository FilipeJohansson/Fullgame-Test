using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour {
    [Header("Health Attributes")]
    [SerializeField] private int maxHealth;
    private int currentHealth;

    [Header("Stamina Attributes")]
    [SerializeField] private int maxStamina;
    private int currentStamina;
    [SerializeField] public float staminaRegen = 2f;
    private float staminaRegenTimer = 0;

    [Header("Ground Attack Attributes")]
    [SerializeField] public int attackDamage = 3;
    [SerializeField] public float attackCooldown = 0.5f;
    private float attackCooldownTimer = 0;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRadius;
    [SerializeField] public bool isAttacking = false;

    [Header("Jump Attack Attributes")]
    [SerializeField] private Transform jumpAttackCheck;
    [SerializeField] private float jumpAttackSizeX;
    [SerializeField] private float jumpAttackSizeY;
    [SerializeField] public bool isJumpAttacking = false;

    [Header("Movement Attributes")]
    public float horizontalMove;

    [Header("Jump Attributes")]
    public bool isJumping = false;
    public bool isInTheAir = false;

    [Header("Dash Attributes")]
    [SerializeField] public float dashCooldown = 0.5f;
    private float dashTimer = 0;
    public bool isDashing = false;

    // [Header("Ground Check Attributes")]
    [Header("Untargetable Attributes")]
    [SerializeField] public float untargetableCooldown = 0.5f;
    private float untargetableTimer = 0;
    public bool isUntargetable = false;

    [Header("Other Attributes")]
    [SerializeField] private LayerMask whatIsEnemy;
    public bool isDead = false;
    public bool isStuned = false;

    [Header("Objects Attributes")]
    [SerializeField] public Rigidbody2D rigidBody;
    public CharacterController2D controller;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public Animator animator;
    [SerializeField] public GameObject stunObject;
    [SerializeField] private SimpleFlash simpleFlash;
    public SlideBar healthBar;
    public SlideBar staminaBar;
    [SerializeField] public Animator camAnimator;

    [Header("Context Attributes")]
    [SerializeField] protected GameManager gameManager;

    public void Awake() {
        simpleFlash = gameObject.GetComponent<SimpleFlash>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        controller = gameObject.GetComponent<CharacterController2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Start() {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        staminaRegenTimer = staminaRegen;
        untargetableTimer = untargetableCooldown;

        healthBar.SetMaxValue(maxHealth);
        staminaBar.SetMaxValue(maxStamina);
    }

    void Update() {
        if (isDead || !gameManager.runningGame)
            return;

        handleMovement();
        HandleAttack();
        RefreshStamina();
        RefreshUntargetable();

        staminaBar.SetValue(currentStamina);
    }

    void FixedUpdate() {
        if (isDead || !gameManager.runningGame)
            return;

        controller.Move(horizontalMove * Time.fixedDeltaTime, isJumping);
        isJumping = false;
    }

    // Private methods
    private void HandleAttack() {
        if (isStuned)
            return;

        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Attack") && attackCooldownTimer <= 0) {
            attackCooldownTimer = attackCooldown;

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

        if (staminaRegenTimer > 0)
            staminaRegenTimer -= Time.deltaTime;

        if (staminaRegenTimer <= 0) {
            currentStamina++;
            staminaRegenTimer = staminaRegen;
        }
    }

    private void RefreshUntargetable() {
        if (isUntargetable && untargetableTimer > 0)
            untargetableTimer -= Time.deltaTime;
        else if (isUntargetable && untargetableTimer <= 0) {
            isUntargetable = false;
            untargetableTimer = untargetableCooldown;
        }
    }

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
            isJumping = true;
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
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, whatIsEnemy);

        foreach (Collider2D enemy in enemies)
            enemy.GetComponent<EnemyBase>().TakeDamage(attackDamage);
    }

    public void JumpAttack() {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(jumpAttackCheck.position, new Vector2(jumpAttackSizeX, jumpAttackSizeY), 0, whatIsEnemy);

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
        rigidBody.velocity = Vector2.zero;
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
        float oldGravity = rigidBody.gravityScale;
        rigidBody.gravityScale = 0;
        rigidBody.AddForce(new Vector2(2 * horizontalMove, 0), ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        rigidBody.gravityScale = 3f;
        isDashing = false;

        yield return null;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);

        Gizmos.DrawWireCube(jumpAttackCheck.position, new Vector3(jumpAttackSizeX, jumpAttackSizeY, 0));
    }
}
