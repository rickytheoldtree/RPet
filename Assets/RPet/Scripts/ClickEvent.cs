using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class ClickEvent : MonoBehaviour
{
    public static readonly UnityEvent<Vector3> OnClick = new UnityEvent<Vector3>();

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int virtualKeyCode);
    private void Update()
    {
        if (LeftMouseDown())
        {
            OnClick.Invoke(Input.mousePosition);
        }
    }
    private static bool LeftMouseDown()
    {
# if UNITY_EDITOR
        return Input.GetMouseButtonDown(0);
#else
        return GetAsyncKeyState(0x01) != 0;
#endif
    }
}