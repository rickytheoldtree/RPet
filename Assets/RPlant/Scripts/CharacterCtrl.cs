using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{
    private Animator animator;
    private static readonly int Walk = Animator.StringToHash("Walk");
    private Coroutine stopWalkCoroutine;
    private Tween rotateTween, headUpTween;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        CharacterSystem.I.CurrentCharacter = this;
    }
    public void OnRightFeetDown()
    {
        
    }
    public void OnLeftFeetDown()
    {
        
    }
    public void SetWalk(bool walk)
    {
        animator.SetBool(Walk, walk);
    }
    public void FaceTo(Vector3 target)
    {
        var direction = target - transform.position;
        direction.y = 0;
        transform.forward = direction;
    }
    public void SetHeadUp(bool headUp)
    {
        headUpTween?.Kill();
        headUpTween = DOTween.To(() => animator.GetLayerWeight(2), x => animator.SetLayerWeight(2, x),
            headUp ? 1 : 0, 0.2f);
    }
    public void RotateTo(Vector3 target)
    {
        rotateTween?.Kill();
        var direction = target - transform.position;
        // 使用DOTween让角色旋转到目标方向
        rotateTween = transform.DOLookAt(target, 0.2f);
    }
    public void StopWalk(Func<bool> condition, Action callback)
    {
        if (stopWalkCoroutine != null)
        {
            StopCoroutine(stopWalkCoroutine);
        }
        stopWalkCoroutine = StartCoroutine(StopWalkWhenCoroutine(condition, callback));
    }
    private IEnumerator StopWalkWhenCoroutine(Func<bool> condition, Action callback)
    {
        while (true)
        {
            yield return null;
            if (condition())
            {
                SetWalk(false);
                break;
            }
        }
        callback?.Invoke();
    }
}
