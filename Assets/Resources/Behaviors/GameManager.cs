using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static int playerGoldAtEndOfLevel = -1;

    private double deathtimer = 1.5;

    private GameObject gameOverUI;
    private GameObject winUI;

    private void Start()
    {
        gameOverUI = GameObject.Find("Game Over UI");
        gameOverUI.SetActive(false);

        winUI = GameObject.Find("Level Won UI");
        winUI.SetActive(false);

        if (playerGoldAtEndOfLevel >= 0)
        {
            Hero.instance.gold = playerGoldAtEndOfLevel;
        }
    }

    void Update () {
        if (Hero.instance.isDead)
        {
            this.deathtimer -= Time.deltaTime;
            if (this.deathtimer <= 0)
            {
                gameOverUI.SetActive(true);
            }
        } else if (Hero.instance.isDone)
        {
            winUI.SetActive(true);
        }
    }

    public void OnTryAgain()
    {
        playerGoldAtEndOfLevel = -1;

		Room.rooms.Clear ();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnNextLevel()
    {
        playerGoldAtEndOfLevel = Hero.instance.gold;

        Room.rooms.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
