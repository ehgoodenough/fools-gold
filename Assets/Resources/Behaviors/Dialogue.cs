using System;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    public static Dialogue instance;

    [SerializeField]
    private Text textField;

    private Action callback;

	void Start () {
        instance = this;
        this.SetVisible(false);
	}

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            this.SetVisible(false);
            callback();
        }
    }

    public void SetText(string text)
    {
        this.textField.text = text;
    }

    public void SetVisible(bool visible)
    {
        this.gameObject.SetActive(visible);
    }

    public void SetCallback(Action callback)
    {
        this.callback = callback;
    }
}
