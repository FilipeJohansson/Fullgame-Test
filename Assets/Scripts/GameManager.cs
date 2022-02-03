using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("Player")]
    public GameObject ts_Player;
    public bool runningGame;

    // Statics References
    static public GameObject Player;

    void Start() {
        Player = ts_Player;
        runningGame = false;
    }

    public void PauseGame() {
        Debug.Log("Game Paused");
    }

    public void StartGame() {
        runningGame = true;
    }
}
