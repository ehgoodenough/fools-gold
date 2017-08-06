using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public void OnStartGame()
    {
        // TODO load level 1
        SceneManager.LoadScene("Other Andrew's Playground");
    }
}
