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

    public void InstantiateArrow() {
        GameObject arrow = GameObject.Instantiate(arrowPrefab, bow.position, Quaternion.identity);
        // arrow.GetComponent<Arrow>().SetDirection(m_FacingRight);
        arrow.GetComponent<Arrow>().SetOwner(bow.gameObject);
        arrow.GetComponent<Arrow>().SetTarget(gameManager.ts_Player);
    }
}
