using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : EnemyBase {
    public GameObject arrowPrefab;
    public Transform bow;

    // Start is called before the first frame update
    public override void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {

    }

    public void InstantiateArrow() {
        GameObject arrow = GameObject.Instantiate(arrowPrefab, bow.position, Quaternion.identity);
        arrow.GetComponent<Arrow>().SetDirection(m_FacingRight);
        // GameObject arrow = GameObject.Instantiate(arrowPrefab, new Vector3(transform.position.x, transform.position.y + .5f, 0), new Quaternion(transform.rotation.x, -180, transform.rotation.z, transform.rotation.w));
        // Rigidbody2D headRb = arrow.GetComponent<Rigidbody2D>();
        // arrow.transform.localScale = transform.localScale;

        // headRb.AddForce(new Vector2(.01f, 1f), ForceMode2D.Impulse);
    }
}
