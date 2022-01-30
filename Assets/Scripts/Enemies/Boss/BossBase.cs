using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : EnemyBase {

    public HealthBar healthBar;

    // Start is called before the first frame update
    public override void Start() {
        base.Start();

        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void FixedUpdate() {
        healthBar.SetHealth(currentHealth);
    }
}
