using System.Collections;
using System.Collections.Generic;
using RicKit.Comon;
using UnityEngine;

public class CharacterSystem : Singleton<CharacterSystem>
{
    public CharacterCtrl CurrentCharacter { get; set; }
    public TargetCtrl Target { get; set; }
    public CharacterSystem()
    {
        InputSystem.OnClick.AddListener(OnClick);
    }

    private void OnClick(Vector3 mousePos)
    {
        if (CurrentCharacter && Target && InputSystem.GetKey(KeyCode.LeftControl))
        {
            Target.MoveTo(mousePos);
            StartWalk();
        }
    }
    private void StartWalk()
    {
        if (CurrentCharacter && Target)
        {
            CurrentCharacter.SetHeadUp(false);
            CurrentCharacter.RotateTo(Target.transform.position);
            CurrentCharacter.SetWalk(true);
            CurrentCharacter.StopWalk(
                () => Vector3.Distance(CurrentCharacter.transform.position, Target.transform.position) < 0.1f
                , FaceCamera);
        }
    }
    private void FaceCamera()
    {
        if (CurrentCharacter)
        {
            CurrentCharacter.RotateTo(Vector3.back * 1000);
            CurrentCharacter.SetHeadUp(true);
        }
    }
}
