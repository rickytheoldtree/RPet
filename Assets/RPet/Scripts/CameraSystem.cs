using System.Collections;
using System.Collections.Generic;
using RicKit.Comon;
using UnityEngine;

namespace RPet
{
    public class CameraSystem : Singleton<CameraSystem>
    {
        public Camera Camera => mCamera ??= Camera.main;
        private Camera mCamera;
        public Vector3 GetMouseWorldPos()
        {
            var pos = Input.mousePosition;
            return Camera.ScreenToWorldPoint(pos);
        }
    }
}

