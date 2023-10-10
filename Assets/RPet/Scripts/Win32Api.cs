
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public static class Win32Api
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
    // 弹窗
    public static void MessageBox(string text, string caption)
    {
#if !UNITY_EDITOR
        MessageBox(IntPtr.Zero, text, caption, 0);
#endif
        Debug.Log($"{caption}: {text}");
    }
}
