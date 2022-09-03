using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    private string message = "fish";

    private void OnMouseEnter()
    {
        ToolTipManager._instance.SetAndShowToolTip(message);
    }

    private void OnMouseExit()
    {
        ToolTipManager._instance.HideToolTip();
    }
}
