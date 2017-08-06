using System;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    public static Dialogue instance;

    [SerializeField]
    private Text textField;

    private AudioSource audioSource;

    private AudioClip clip;
    private Action callback;

    private float openedTime;

    private void Awake()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        instance = this;
        this.SetVisible(false);
    }

    private void Update()
    {
        if (Input.anyKeyDown && Time.time - openedTime >= 1.0f)
        {
            this.SetVisible(false);
            if (callback != null)
            {
                callback();
            }
        }
    }

    public void SetText(string text)
    {
        this.textField.text = text;
    }

    public bool IsVisible()
    {
        return this.gameObject.activeSelf;
    }

    private void SetVisible(bool visible)
    {
        this.gameObject.SetActive(visible);
    }

    public void Show()
    {
        openedTime = Time.time;
        this.SetVisible(true);
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void SetAudioClip(AudioClip clip)
    {
        this.clip = clip;
    }

    public void SetCallback(Action callback)
    {
        this.callback = callback;
    }
}
