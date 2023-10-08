using RPet;
using UnityEngine;

public class TargetCtrl : MonoBehaviour
{
    private void Awake()
    {
        CharacterSystem.I.Target = this;
    }
    public void MoveTo(Vector3 mousePos)
    {
        //找到鼠标点击的位置在世界坐标原点平面上的位置
        var worldPosition = CameraSystem.I.Camera.ScreenToWorldPoint(mousePos);
        var depth = worldPosition.y * Mathf.Pow(2, 0.5f);
        mousePos.z = depth;
        worldPosition = CameraSystem.I.Camera.ScreenToWorldPoint(mousePos);
        transform.position = worldPosition;
    }
}
