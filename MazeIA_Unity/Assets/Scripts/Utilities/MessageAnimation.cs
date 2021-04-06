using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageAnimation : MonoBehaviour
{
    public enum Colors
    {
        Success,
        Info,
        Warning,
        Error,
    }
    private Color[] colors = { new Color(0.6f, 0.6f, 1f), new Color(0.6f, 1f, 0.6f), new Color(1f, 0.8f, 0.5f), new Color(1f, 0.6f, 0.6f) };

    [SerializeField] Canvas canvas;
    [SerializeField] Image background;
    [SerializeField] Text titleText;
    [SerializeField] Text messageTxt;

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        transform.SetParent(canvas.transform);
    }
    public void SetUpMessage(string message, Color color)
    {
        background.color = color;
        this.messageTxt.text = message;
    }
    public void SetUpMessage(string title, string message, Colors color)
    {
        background.color = colors[(int)color];
        this.titleText.text = title;
        this.messageTxt.text = message;
    }
    public void SetUpMessage(string title, string message, Colors color, Transform transform)
    {
        background.color = colors[(int)color];
        this.titleText.text = title;
        this.messageTxt.text = message;
        this.transform.SetParent(transform);
    }
}
