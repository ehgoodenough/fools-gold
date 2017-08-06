﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static int currentLevel; // NOTE: Zero indexed

    private static int playerGoldAtEndOfLevel = -1;
    private static bool gameStart = true;

    public static GameManager instance;

    private double deathtimer = 1.5;
    private bool endLevelDialogueShown = false;

    private GameObject gameOverUI;
    private GameObject winUI;

	public static event Action onGameStart;

    void Start()
    {
        instance = this;

		if (onGameStart != null)
			onGameStart();

		gameOverUI = GameObject.Find("Game Over UI");
        gameOverUI.SetActive(false);

        winUI = GameObject.Find("Level Won UI");
        winUI.SetActive(false);

        if (playerGoldAtEndOfLevel >= 0)
        {
            Hero.instance.gold = playerGoldAtEndOfLevel;
        }

        if (gameStart)
        {
            Dialogue.instance.SetText("Greetings, hero! Only YOU can save the princess, and I'll help you find her... for a small fee, of course...");
            Dialogue.instance.SetCallback(delegate
            {
                gameStart = false;
            });
            Dialogue.instance.SetVisible(true);
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
        } else if (Hero.instance.isDone && !endLevelDialogueShown)
        {
            Dialogue.instance.SetText("Ah, hero, the princess was JUST here. I promise. For a small fee, I can take you to the castle where she is now...");
            Dialogue.instance.SetCallback(delegate
            {
                endLevelDialogueShown = true;
                winUI.SetActive(true);
            });
            Dialogue.instance.SetVisible(true);
        }
    }

    public void OnTryAgain()
    {
        UIAudioSource.instance.Play();

        // Start back at the beginning, or just the current level?
        // currentLevel = 0;
        playerGoldAtEndOfLevel = -1;

		Room.rooms.Clear ();
        Invoke("Reload", .5f);
    }

    public void OnNextLevel()
    {
        UIAudioSource.instance.Play();

        currentLevel = ++currentLevel % 3;
        playerGoldAtEndOfLevel = Hero.instance.gold;

        Room.rooms.Clear();

        Invoke("Reload", .5f);
    }

    public void OnQuit()
    {
        UIAudioSource.instance.Play();
        Invoke("Quit", .5f);
    }

    private void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Quit()
    {
        Application.Quit();
    }

    public bool IsPaused()
    {
        return Hero.instance.isDead || Hero.instance.isDone || Dialogue.instance.IsVisible();
    }

    public int GoldNeeded()
    {
        switch (currentLevel)
        {
            case 0:
                return 5;
            case 1:
                return 7;
            case 2:
                return 10;
            default:
                return 5;
        }
    }

    public void ShowNotEnoughGoldDialogue()
    {
        Dialogue.instance.SetText(string.Format("What's this? Not enough coin! Don't come back until you have {0} gold!", GoldNeeded()));
        Dialogue.instance.SetCallback(null);
        Dialogue.instance.SetVisible(true);
    }
}
