using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttack : MonoBehaviour, IAttack  {

    protected GameObject owner;

    public SimpleAttack(GameObject _owner) {
        owner = _owner;
    }

    public void Attack(MonoBehaviour mono) {
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            // Debug.Log("Hit");
        }
    }

}
