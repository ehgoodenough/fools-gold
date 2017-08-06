using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static int playerGoldAtEndOfLevel = -1;
    private static bool gameStart = true;

    public static GameManager instance;

    private double deathtimer = 1.5;

    private GameObject gameOverUI;
    private GameObject winUI;

    void Start()
    {
        instance = this;
        if (gameStart)
        {
            Dialogue.instance.SetText("Greetings, hero! Only YOU can save the princess, and I'll help you find her... for a small fee, of course...");
            Dialogue.instance.SetCallback(delegate
            {
                gameStart = false;
            });
            Dialogue.instance.SetVisible(true);
        }
    
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

    public bool IsPaused()
    {
        return Hero.instance.isDead || Hero.instance.isDone || gameStart;
    }
}
