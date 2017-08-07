using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static int currentLevel; // NOTE: Zero indexed
    private const int AMOUNT_OF_LEVELS = 3;

    private static int playerGoldAtEndOfLevel = -1;
    private static bool gameStart = true;

    public static GameManager instance;

    private double deathtimer = 1.5;
    private bool endLevelDialogueShown = false;

    private AudioClip introClip;
    private AudioClip victoryClip;
    private AudioClip notEnoughGoldClip;

    private GameObject gameOverUI;
    private GameObject winUI;

	public static event Action onGameStart;

    private AudioSource[] musics;

	private int goldInterest;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        introClip = Resources.Load("Audio/Speech_Intro") as AudioClip;
        victoryClip = Resources.Load("Audio/Speech_Victory") as AudioClip;
        notEnoughGoldClip = Resources.Load("Audio/Speech_Not_Enough_Coin") as AudioClip;

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
            Dialogue.instance.SetAudioClip(introClip);
            Dialogue.instance.SetCallback(delegate
            {
                gameStart = false;
            });
            Dialogue.instance.Show();
        }

        musics = GetComponents<AudioSource>();
        Debug.Log(musics.Length + ", " + GameManager.currentLevel);
        musics[GameManager.currentLevel].Play();
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
            endLevelDialogueShown = true;
            
            switch (currentLevel)
            {
                case 0:
                    Dialogue.instance.SetText(
                        string.Format("Great work, brave adventurer, but the princess is in another castle! I can take you there for a few gold pieces... (-{0} gold)", GoldNeeded()));
                    break;
                case 1:
                    Dialogue.instance.SetText(
                        string.Format("Hero! You'll never believe what happened. The princess was kidnapped AGAIN. For a bit of coin, I can show you where she went... (-{0} gold)", GoldNeeded()));
                    break;
                case 2:
                    Dialogue.instance.SetText(
                        "Ah, hero, the princess was JUST here. I promise. For a small fee, I can take you to the castle where she is now...");
                    break;
            }
            Dialogue.instance.SetAudioClip(victoryClip);
            Dialogue.instance.SetCallback(delegate
            {
                winUI.SetActive(true);
            });
            Dialogue.instance.Show();
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

        currentLevel = ++currentLevel % AMOUNT_OF_LEVELS;
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
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            Application.Quit();
        }
    }

    public bool IsPaused()
    {
        return Hero.instance.isDead || Hero.instance.isDone || Dialogue.instance.IsVisible();
    }

    public int GoldNeeded()
    {
		if (currentLevel == 2) {
			goldInterest++;
			return (int)(Map.instance.numGoldOnMap) + (goldInterest * goldInterest);
		} else {
			return (int)(Map.instance.numGoldOnMap * 0.75f);
		}
    }

    public void ShowNotEnoughGoldDialogue()
    {

        switch (currentLevel)
        {
            case 0:
                Dialogue.instance.SetText(
                    string.Format("Hero, I require more coin to bring you to the princess. Bring me {0} gold pieces!", GoldNeeded()));
                break;
            case 1:
                Dialogue.instance.SetText(
                    string.Format("What's this? Not enough coin! Don't come back until you have {0} gold pieces!", GoldNeeded()));
                break;
            case 2:
                Dialogue.instance.SetText(
                    string.Format("Brave hero, I now require {0} gold pieces to find the princess. There must be some more gold around here somewhere...", GoldNeeded()));
                break;
        }
        Dialogue.instance.SetAudioClip(notEnoughGoldClip);
        Dialogue.instance.SetCallback(null);
        Dialogue.instance.Show();
    }
}
