using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : EnemyBase {

    public SlideBar healthBar;
    [SerializeField] GameObject bossHead;

    // Start is called before the first frame update
    public override void Start() {
        base.Start();

        healthBar.SetMaxValue(maxHealth);
    }

    // Update is called once per frame
    void FixedUpdate() {
        healthBar.SetValue(currentHealth);
    }

    public void DashAttack() {
        float xForce = m_FacingRight ? -30 : 30;
        rb.AddForce(new Vector2(xForce, -10), ForceMode2D.Impulse);
    }

    public override void Die() {
        InstantiateHead();
        base.Die();
    }

    public void InstantiateHead() {
        GameObject head = GameObject.Instantiate(bossHead, new Vector3(transform.position.x, transform.position.y + 2, 0), new Quaternion(transform.rotation.x, -180, transform.rotation.z, transform.rotation.w));
        Rigidbody2D headRb = head.GetComponent<Rigidbody2D>();
        head.transform.localScale = transform.localScale;

        headRb.AddForce(new Vector2(.02f, 3), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "PlayerDamage")
            other.gameObject.GetComponentInParent<PlayerBase>().TakeDamage(collisionDamage);
    }
}
