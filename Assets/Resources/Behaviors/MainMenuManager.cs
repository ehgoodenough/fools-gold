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
        UIAudioSource.instance.Play();
    }

    public void OnCredits()
    {
        menu.SetActive(false);
        credits.SetActive(true);
        UIAudioSource.instance.Play();
    }

    public void OnBack()
    {
        menu.SetActive(true);
        credits.SetActive(false);
        UIAudioSource.instance.Play();
    }

    public void OnAdventurer()
    {
        Application.OpenURL("http://www.pentacom.jp/pentacom/bitfontmaker2/gallery/?id=195");
    }

    public void OnCreativeCommons()
    {
        Application.OpenURL("https://creativecommons.org/licenses/by/4.0/legalcode");
    }

    public void OnAndrewH()
    {
        Application.OpenURL("https://www.linkedin.com/in/andrew-hermus-ab1672a5");
    }

    public void OnAndrewM()
    {
        Application.OpenURL("https://twitter.com/ehgoodenough");
    }

    public void OnMatan()
    {
        Application.OpenURL("https://onehamsa.com/");
    }

    public void OnRobert()
    {
        Application.OpenURL("https://www.linkedin.com/in/ackleyrobert/");
    }

    public void OnVictor()
    {
        Application.OpenURL("https://www.instagram.com/thermalamped/");
    }

    public void OnKyle()
    {
        Application.OpenURL("https://soundcloud.com/kylemartinvgmusic");
    }
}
