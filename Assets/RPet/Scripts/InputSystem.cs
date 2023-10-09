using System;
using RicKit.Comon;
using UnityEngine;
using UnityEngine.Events;
using UnityRawInput;

public class InputSystem : MonoSingleton<InputSystem>
{
    public static readonly UnityEvent<Vector3> OnClick = new UnityEvent<Vector3>();

    /*[DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int virtualKeyCode);*/
    protected override void GetAwake()
    {
#if !UNITY_EDITOR
        RawInput.Start();
        RawInput.WorkInBackground = true;
        RawInput.InterceptMessages = false;
        RawInput.OnKeyDown += OnRawKeyDown;
#endif
    }

    private bool rawLeftButtonDown;
    private void OnRawKeyDown(RawKey key)
    {
        if(key == RawKey.LeftButton)
        {
            rawLeftButtonDown = true;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || rawLeftButtonDown)
        {
            OnClick.Invoke(Input.mousePosition);
            rawLeftButtonDown = false;
        }
    }
    public static bool GetKey(KeyCode keyCode)
    {
        if (Input.GetKey(keyCode))
            return true;
#if !UNITY_EDITOR
        switch (keyCode)
        {
            default: return false;
            case KeyCode.LeftShift: return RawInput.IsKeyDown(RawKey.LeftShift);
            case KeyCode.LeftControl: return RawInput.IsKeyDown(RawKey.LeftControl);
        }
#endif
        return false;
    }
}