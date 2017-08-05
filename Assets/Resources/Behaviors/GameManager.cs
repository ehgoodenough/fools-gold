using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private double deathtimer = 1.5;

    private GameObject gameOverUI;

    private void Start()
    {
        gameOverUI = GameObject.Find("Game Over UI");
        gameOverUI.SetActive(false);
    }

    void Update () {
        if (Hero.instance.isDead)
        {
            this.deathtimer -= Time.deltaTime;
            if (this.deathtimer <= 0)
            {
                gameOverUI.SetActive(true);
            }
        }
    }

    public void OnTryAgain()
    {
		Room.rooms.Clear ();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
