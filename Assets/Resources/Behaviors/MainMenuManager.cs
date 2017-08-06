using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject credits;

    public void OnStartGame()
    {
        SceneManager.LoadScene("Dungeon");
    }

    public void OnCredits()
    {
        menu.SetActive(false);
        credits.SetActive(true);
    }

    public void OnBack()
    {
        menu.SetActive(true);
        credits.SetActive(false);
    }
}
