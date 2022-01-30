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
    [SerializeField] public float damage;
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

    public bool jump = false;
    public bool isInTheAir = false;

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        healthBar.SetMaxValue(maxHealth);
        staminaBar.SetMaxValue(maxStamina);

        simpleFlash = gameObject.GetComponent<SimpleFlash>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        horizontalMove = Input.GetAxisRaw("Horizontal") * controller.runSpeed;

        // animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump")) {
            jump = true;
            isInTheAir = true;
        }

        if (Input.GetButtonDown("Attack"))
            if (isInTheAir)
                isJumpAttacking = true;
            else
                isAttacking = true;
    }

    public void onLanding() {
        isInTheAir = false;
        isAttacking = false;
        isJumpAttacking = false;
    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;

        staminaBar.SetValue(currentStamina);
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;
        healthBar.SetValue(currentHealth);

        simpleFlash.Flash();

        if (currentHealth <= 0)
            Die();
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
        Debug.Log("Player died");
        StartCoroutine(DeathAnimation(100));
    }

    IEnumerator DeathAnimation(float duration) {
        Color deathColor = spriteRenderer.color;
        deathColor.a = 0;
        // deathColor.a = 0;
        for (float t = 0f; t < duration; t += Time.deltaTime) {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, deathColor, normalizedTime);
            yield return null;
        }
        spriteRenderer.color = deathColor; //without this, the value will end at something like 0.9992367
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_AttackCheck.position, k_AttackRadius);

        Gizmos.DrawWireCube(m_JumpAttackCheck.position, new Vector3(k_JumpAttackSizeX, k_JumpAttackSizeY, 0));
    }
}
