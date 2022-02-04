using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IAttack {
    protected GameObject owner;
    public int attackDamage = 10;
    public float speed = 10f;

    public float timeToDestroy = 5;

    void Update() {
        transform.Translate(Vector2.right * Time.deltaTime * speed);

        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy <= 0)
            Destroy(gameObject);
    }

    public void Attack(MonoBehaviour mono) {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "PlayerDamage") {
            other.GetComponentInParent<PlayerBase>().TakeDamage(attackDamage);
            Destroy(this.gameObject);
        }
    }

    public void SetDirection(bool facingRight) {
        if (facingRight)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
