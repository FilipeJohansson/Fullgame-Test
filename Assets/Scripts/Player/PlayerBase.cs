using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour {
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public int maxHealth;
    [SerializeField] public int currentHealth;
    [SerializeField] public float damage;
    [SerializeField] public bool isAttacking = false;

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update() {

    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    public void Die() {
        // Destroy(gameObject);
        Debug.Log("Player died");
    }
}
