using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    public Hero hero;

    public Image[] hearts;
    public Text goldText;

    public Sprite filledHeart;
    public Sprite emptyHeart;

	void Update () {
        // TODO use real values once they are available
        int health = 3; // hero.GetHealth()
        int gold = 100; // hero.GetGold()

        for (int i = 0; i < health; i++)
        {
            hearts[i].sprite = filledHeart;
        }
        for (int i = health; i < hearts.Length; i++)
        {
            hearts[i].sprite = emptyHeart;
        }

        goldText.text = gold.ToString();
	}
}
