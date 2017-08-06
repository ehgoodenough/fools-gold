using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioSource : MonoBehaviour {

    public static UIAudioSource instance;

	void Start () {
        instance = this;
	}

    public void Play()
    {
        this.gameObject.GetComponent<AudioSource>().Play();
    }
}
