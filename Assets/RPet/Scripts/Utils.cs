using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 GetMouseWorldPos()
    {
        var pos = Input.mousePosition;
        pos.z = 10;
        return Camera.main.ScreenToWorldPoint(pos);
    }
}
