using System.Collections;
using System.Collections.Generic;
using RicKit.Comon;
using TMPro;
using UnityEngine;

public class DebugText : MonoSingleton<DebugText>
{
    TextMeshProUGUI text;

    protected override void GetAwake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    public void Log(string msg)
    {
        text.text = msg;
    }
}
