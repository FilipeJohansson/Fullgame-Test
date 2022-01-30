using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoScript : MonoBehaviour {
    public float timeBtwSpawns;
    public float startTimeBtwSpawns;

    public GameObject echo;
    public PlayerBase player;

    private void Update() {
        if (!player.isDashing)
            return;

        if (timeBtwSpawns <= 0) {
            GameObject instance = Instantiate(echo, transform.position, Quaternion.identity);
            Destroy(instance, 1f);
            timeBtwSpawns = startTimeBtwSpawns;
        } else
            timeBtwSpawns -= Time.deltaTime;
    }
}
