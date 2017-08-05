using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    public Image[] hearts;
    public Text goldText;

    public Sprite filledHeart;
    public Sprite emptyHeart;

    void Update () {
        int health = Hero.instance.health;
        int gold = Hero.instance.gold;
        int maxHealthDiff = hearts.Length - Hero.instance.maxhealth;

        for (int i = 0; i < maxHealthDiff; i++)
        {
            hearts[i].enabled = false;
        }
        for (int i = maxHealthDiff; i < health + maxHealthDiff; i++)
        {
            hearts[i].sprite = filledHeart;
        }
        for (int i = health + maxHealthDiff; i < hearts.Length; i++)
        {
            hearts[i].sprite = emptyHeart;
        }

        goldText.text = gold.ToString();
	}
}
