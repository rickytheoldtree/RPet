using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{
    private Animator animator;
    private static readonly int Walk = Animator.StringToHash("Walk");
    private Tween rotateTween, headUpTween;
    private Coroutine walkCoroutine;

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

    private void Update()
    {
        if (InputSystem.GetKey(KeyCode.LeftShift))
        {
            Time.timeScale = 2;
        }
        else
        {
            Time.timeScale = 1;
        }
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
        rotateTween = transform.DOLookAt(target, 0.2f);
    }

    public void WalkTo(Vector3 target, Action callback)
    {
        if (walkCoroutine != null)
        {
            StopCoroutine(walkCoroutine);
        }
        walkCoroutine = StartCoroutine(WalkToCoroutine(target, callback));
    }
    private IEnumerator WalkToCoroutine(Vector3 target, Action callback)
    {
        var startPos = transform.position;
        var targetDistance = Vector3.Distance(startPos, target);
        while (true)
        {
            var distance = Vector3.Distance(startPos, transform.position);
            if (distance >= targetDistance)
            {
                SetWalk(false);
                break;
            }
            yield return null;
        }
        callback?.Invoke();
    }
}
