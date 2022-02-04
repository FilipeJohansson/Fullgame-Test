using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StaminaAttributes {
    [SerializeField] public int maxStamina = 5;
    [HideInInspector] public int currentStamina;
    [Tooltip("Time to recover stamina")]
    [SerializeField] public float staminaRegenCooldown = 4f;
    [HideInInspector] public float staminaRegenTimer;
}

[System.Serializable]
public class GroundAttackAttributes {
    [SerializeField] public int attackDamage = 25;
    [Tooltip("Time to can attack again")]
    [SerializeField] public float attackCooldown = 0.5f;
    [HideInInspector] public float attackTimer;
    [Tooltip("Transform of check attack range")]
    [SerializeField] public Transform attackCheck;
    [SerializeField] public float attackRadius;
    [SerializeField] public bool isAttacking = false;
}

[System.Serializable]
public class JumpAttackAttributes {
    [Tooltip("Transform of check jump attack range")]
    [SerializeField] public Transform jumpAttackCheck;
    [SerializeField] public float jumpAttackSizeX;
    [SerializeField] public float jumpAttackSizeY;
    [SerializeField] public bool isJumpAttacking = false;
}

[System.Serializable]
public class ObjectsAttributes {
    [SerializeField] public Rigidbody2D rigidBody;
    [SerializeField] public CharacterController2D controller;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public Animator animator;
    [SerializeField] public GameObject stunObject;
    [SerializeField] public SimpleFlash simpleFlash;
    [SerializeField] public SlideBar healthBar;
    [SerializeField] public SlideBar staminaBar;
    [SerializeField] public Animator camAnimator;
}

public class PlayerBase : MonoBehaviour {

    [Header("Health Attributes")]
    [SerializeField] private int maxHealth;
    private int currentHealth;

    [Header("Stamina")]
    public StaminaAttributes staminaAttributes;

    [Header("Ground Attack")]
    public GroundAttackAttributes groundAttackAttributes;

    [Header("Jump Attack")]
    public JumpAttackAttributes jumpAttackAttributes;

    [Header("Movement Attributes")]
    [HideInInspector] public float horizontalMove;

    [Header("Jump Attributes")]
    public bool isJumping = false;
    public bool isInTheAir = false;

    [Header("Dash Attributes")]
    [Tooltip("Time to can dash again")]
    [SerializeField] public float dashCooldown = 0.5f;
    private float dashTimer;
    public bool isDashing = false;

    [Header("Untargetable Attributes")]
    [Tooltip("Time to can be targetable again")]
    [SerializeField] public float untargetableCooldown = 1f;
    private float untargetableTimer;
    public bool isUntargetable = false;

    [Header("Other Attributes")]
    [Tooltip("LayerMask of the enemies")]
    [SerializeField] private LayerMask whatIsEnemy;
    public bool isDead = false;
    public bool isStuned = false;

    [Header("Objects")]
    public ObjectsAttributes objectsAttributes;

    [Header("Context Attributes")]
    [SerializeField] protected GameManager gameManager;

    public void Awake() {
        objectsAttributes.simpleFlash = gameObject.GetComponent<SimpleFlash>();
        objectsAttributes.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        objectsAttributes.rigidBody = gameObject.GetComponent<Rigidbody2D>();
        objectsAttributes.controller = gameObject.GetComponent<CharacterController2D>();
        objectsAttributes.animator = gameObject.GetComponent<Animator>();

        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
    }

    void Start() {
        currentHealth = maxHealth;
        staminaAttributes.currentStamina = staminaAttributes.maxStamina;
        staminaAttributes.staminaRegenTimer = staminaAttributes.staminaRegenCooldown;
        untargetableTimer = untargetableCooldown;

        objectsAttributes.healthBar.SetMaxValue(maxHealth);
        objectsAttributes.staminaBar.SetMaxValue(staminaAttributes.maxStamina);
    }

    void Update() {
        if (isDead || !gameManager.runningGame)
            return;

        handleMovement();
        HandleAttack();
        RefreshStamina();
        RefreshUntargetable();

        objectsAttributes.staminaBar.SetValue(staminaAttributes.currentStamina);
    }

    void FixedUpdate() {
        if (isDead || !gameManager.runningGame)
            return;

        objectsAttributes.controller.Move(horizontalMove * Time.fixedDeltaTime, isJumping);
        isJumping = false;
    }

    // Private methods
    private void HandleAttack() {
        if (isStuned)
            return;

        if (groundAttackAttributes.attackTimer > 0)
            groundAttackAttributes.attackTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Attack") && groundAttackAttributes.attackTimer <= 0) {
            groundAttackAttributes.attackTimer = groundAttackAttributes.attackCooldown;

            if (isInTheAir) {
                if (staminaAttributes.currentStamina > 0) {
                    jumpAttackAttributes.isJumpAttacking = true;

                    staminaAttributes.currentStamina--;
                    objectsAttributes.staminaBar.SetValue(staminaAttributes.currentStamina);
                }
            } else
                groundAttackAttributes.isAttacking = true;
        }
    }

    private void RefreshStamina() {
        if (staminaAttributes.currentStamina == staminaAttributes.maxStamina)
            return;

        if (staminaAttributes.staminaRegenTimer > 0)
            staminaAttributes.staminaRegenTimer -= Time.deltaTime;

        if (staminaAttributes.staminaRegenTimer <= 0) {
            staminaAttributes.currentStamina++;
            staminaAttributes.staminaRegenTimer = staminaAttributes.staminaRegenCooldown;
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
            if (staminaAttributes.currentStamina > 0) {
                dashTimer = dashCooldown;

                staminaAttributes.currentStamina--;
                objectsAttributes.staminaBar.SetValue(staminaAttributes.currentStamina);
                StartCoroutine(DashCoroutine(0.1f));
            }
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * objectsAttributes.controller.runSpeed;

        if (Input.GetButtonDown("Jump")) {
            isJumping = true;
            isInTheAir = true;
        }
    }

    // Public methods
    public void onLanding() {
        isInTheAir = false;
        groundAttackAttributes.isAttacking = false;
        jumpAttackAttributes.isJumpAttacking = false;
    }

    public void TakeDamage(int amount) {
        if (isUntargetable)
            return;

        objectsAttributes.camAnimator.SetTrigger("Shake");

        isUntargetable = true;
        untargetableTimer = untargetableCooldown;

        currentHealth -= amount;
        objectsAttributes.healthBar.SetValue(currentHealth);

        objectsAttributes.simpleFlash.Flash();

        if (currentHealth <= 0)
            Die();
    }

    public void Stun(float duration) {
        if (isUntargetable)
            return;

        horizontalMove = 0;
        isStuned = true;
        objectsAttributes.stunObject.SetActive(true);
        StartCoroutine(StunCoroutine(duration));
    }

    public void Attack() {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(groundAttackAttributes.attackCheck.position, groundAttackAttributes.attackRadius, whatIsEnemy);

        foreach (Collider2D enemy in enemies)
            enemy.GetComponent<EnemyBase>().TakeDamage(groundAttackAttributes.attackDamage);
    }

    public void JumpAttack() {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(jumpAttackAttributes.jumpAttackCheck.position, new Vector2(jumpAttackAttributes.jumpAttackSizeX, jumpAttackAttributes.jumpAttackSizeY), 0, whatIsEnemy);

        foreach (Collider2D enemy in enemies)
            enemy.GetComponent<EnemyBase>().TakeDamage(groundAttackAttributes.attackDamage);

        objectsAttributes.controller.ApplyNegativeForce();
    }

    public void ResetAttack() {
        groundAttackAttributes.isAttacking = false;
        jumpAttackAttributes.isJumpAttacking = false;
    }

    public void Die() {
        isDead = true;
        objectsAttributes.rigidBody.velocity = Vector2.zero;
        gameManager.runningGame = false;
        StartCoroutine(DeathAnimation(100));
    }

    IEnumerator DeathAnimation(float duration) {
        Color deathColor = objectsAttributes.spriteRenderer.color;
        deathColor.a = 0;

        for (float t = 0f; t < duration; t += Time.deltaTime) {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            objectsAttributes.spriteRenderer.color = Color.Lerp(objectsAttributes.spriteRenderer.color, deathColor, normalizedTime);
            yield return null;
        }
        objectsAttributes.spriteRenderer.color = deathColor; //without this, the value will end at something like 0.9992367

        gameManager.PauseGame();
    }

    IEnumerator StunCoroutine(float duration) {
        yield return new WaitForSeconds(duration);
        isStuned = false;
        objectsAttributes.stunObject.SetActive(false);
        yield return null;
    }

    IEnumerator DashCoroutine(float duration) {
        isDashing = true;
        isUntargetable = true;
        float oldGravity = objectsAttributes.rigidBody.gravityScale;
        objectsAttributes.rigidBody.gravityScale = 0;
        objectsAttributes.rigidBody.AddForce(new Vector2(2 * horizontalMove, 0), ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        objectsAttributes.rigidBody.gravityScale = 3f;
        isDashing = false;

        yield return null;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundAttackAttributes.attackCheck.position, groundAttackAttributes.attackRadius);

        Gizmos.DrawWireCube(jumpAttackAttributes.jumpAttackCheck.position, new Vector3(jumpAttackAttributes.jumpAttackSizeX, jumpAttackAttributes.jumpAttackSizeY, 0));
    }
}
