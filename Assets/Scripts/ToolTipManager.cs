using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager _instance;
    public TextMeshProUGUI text;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = Camera.current.ScreenToViewportPoint(Mouse.current.position.ReadValue());
    }

    public void SetAndShowToolTip(string toolTipText)
    {
        gameObject.SetActive(true);
        text.text = toolTipText;
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
        text.text = string.Empty;
    }
}
