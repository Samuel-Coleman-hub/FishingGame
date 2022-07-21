using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    private float fps;
    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating("GetFPS", 1, 1);
    }
    
    public void GetFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        text.text = fps.ToString();
    }
}
