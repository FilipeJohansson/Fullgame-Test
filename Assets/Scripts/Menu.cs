using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    public GameManager gameManager;

    public GameObject mainMenu;
    public GameObject gameOverMenu;

    public GameObject bossHealthBar;
    public GameObject playerHealthBar;
    public GameObject playerStaminaBar;

    public bool isMainMenuActive;

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();

        GameOverMenu(false);
        MainMenu(true);
        bossHealthBar.SetActive(false);
        playerHealthBar.SetActive(false);
        playerStaminaBar.SetActive(false);
    }

    void Update() {
        if (!gameManager.runningGame && !isMainMenuActive) {
            GameOverMenu(true);
            bossHealthBar.SetActive(false);
            playerHealthBar.SetActive(false);
            playerStaminaBar.SetActive(false);
        }
    }

    public void StartGame() {
        gameManager.StartGame();
        MainMenu(false);
        bossHealthBar.SetActive(true);
        playerHealthBar.SetActive(true);
        playerStaminaBar.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene("MainScene");
    }

    public void Quit() {
        Application.Quit();
    }

    public void GameOverMenu(bool state) {
        gameOverMenu.SetActive(state);
    }

    public void MainMenu(bool state) {
        mainMenu.SetActive(state);
        isMainMenuActive = state;
    }
}
