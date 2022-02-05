using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IAttack {
    protected GameObject owner;
    protected GameObject target;

    private float targetPosX;
    private float targetPosY;

    private float ownerX;
    private float targetX;

    private float dist;
    private float nextX;
    private float baseY;
    private float height;

    public Vector3 movePosition;

    private Rigidbody2D rB;
    public int attackDamage = 10;
    public float speed = 10f;

    public float timeToDestroy = 5;

    void Start() {
        rB = GetComponent<Rigidbody2D>();

        targetPosX = target.transform.position.x;
        targetPosY = target.transform.position.y;

        ownerX = owner.transform.position.x;
        targetX = target.transform.position.x;
        dist = targetX - ownerX;
    }

    void Update() {
        // transform.Translate(Vector2.right * Time.deltaTime * speed);
        // rB.velocity = transform.right * speed;

        timeToDestroy -= Time.deltaTime;

        Vector3 toPos = new Vector3(targetPosX, targetPosY, 0);

        if (movePosition == toPos)
             Debug.Log("Arrow reached target");

        if (movePosition == target.transform.position || timeToDestroy <= 0)
            Destroy(gameObject);
    }

    void FixedUpdate() {
        nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        baseY = Mathf.Lerp(owner.transform.position.y, target.transform.position.y, (nextX - ownerX) / dist);
        height = 2 * (nextX - ownerX) * (nextX - targetX) / (-0.25f * dist * dist);

        movePosition = new Vector3(nextX, baseY + height, transform.position.z);

        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;
    }

    public void Attack(MonoBehaviour mono) {
        throw new System.NotImplementedException();
    }

    private static Quaternion LookAtTarget(Vector2 r) {
        return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
    }

    public void SetDirection(bool facingRight) {
        if (facingRight)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void SetTarget(GameObject target) {
        this.target = target;
    }

    public void SetOwner(GameObject owner) {
        this.owner = owner;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "PlayerDamage") {
            other.GetComponentInParent<PlayerBase>().TakeDamage(attackDamage);
            Destroy(this.gameObject);
        }
    }
}
