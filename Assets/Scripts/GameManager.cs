using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("Player")]
    public GameObject ts_Player;

    // Statics References
    static public GameObject Player;

    void Start() {
        Player = ts_Player;
    }
}
