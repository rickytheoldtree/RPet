using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RicKit.Comon;
using UnityEngine;
using UnityEngine.Events;

public class InputSystem : MonoSingleton<InputSystem>
{
    public static readonly UnityEvent<Vector3> OnClick = new UnityEvent<Vector3>();

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int virtualKeyCode);
    protected override void GetAwake()
    {
        
    }
    private void Update()
    {
        if (LeftMouseDown())
        {
            OnClick.Invoke(Input.mousePosition);
        }

        if (GetKey(KeyCode.LeftShift))
        {
            Time.timeScale = 2;
        }
        else
        {
            Time.timeScale = 1;
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

    public static bool GetKey(KeyCode keyCode)
    {
#if UNITY_EDITOR
        return Input.GetKey(keyCode);
#endif
        switch (keyCode)
        {
            case KeyCode.W:
                return GetAsyncKeyState(0x57) != 0;
            case KeyCode.A:
                return GetAsyncKeyState(0x41) != 0;
            case KeyCode.S:
                return GetAsyncKeyState(0x53) != 0;
            case KeyCode.D:
                return GetAsyncKeyState(0x44) != 0;
            case KeyCode.LeftControl:
                return GetAsyncKeyState(0xA2) != 0;
            case KeyCode.LeftShift:
                return GetAsyncKeyState(0xA0) != 0;
            default:
                return false;
        }
    }
}