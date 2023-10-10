using System;
using RicKit.Comon;
using UnityEngine;
using UnityEngine.Events;
using UnityRawInput;

public class InputSystem : MonoSingleton<InputSystem>
{
    public static readonly UnityEvent<Vector3> OnLeftButtonDown = new UnityEvent<Vector3>();
    
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
#if !UNITY_EDITOR
        private void OnRawKeyDown(RawKey key)
    {
        if(key == RawKey.LeftButton)
        {
            rawLeftButtonDown = true;
        }
    }
#endif
    private void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        RawInput.Stop();
#endif
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || rawLeftButtonDown)
        {
            OnLeftButtonDown.Invoke(Input.mousePosition);
            rawLeftButtonDown = false;
        }
    }
    public static bool GetKey(KeyCode keyCode)
    {
        return Input.GetKey(keyCode) || GetRawKey(keyCode);
    }
    private static bool GetRawKey(KeyCode keyCode)
    {
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