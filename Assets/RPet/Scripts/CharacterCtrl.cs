using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{
    private Animator animator;
    private static readonly int Walk = Animator.StringToHash("Walk");
    private Tween rotateTween, headUpTween;
    private Coroutine walkCoroutine;

    private TargetCtrl Target => target ??= new GameObject("Target").AddComponent<TargetCtrl>();
    private TargetCtrl target;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        InputSystem.OnLeftButtonDown.AddListener(OnClick);
    }

    private void Start()
    {
        Win32Api.MessageBox("按住Ctrl键点击地面，角色会自动走到目标位置；按下Shift人物动作会加速", "操作说明");
        FaceCamera();
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
    

    private void OnClick(Vector3 mousePos)
    {
        if (Target && InputSystem.GetKey(KeyCode.LeftControl))
        {
            Target.MoveTo(mousePos);
            StartWalk();
        }
    }
    private void StartWalk()
    {
        if (Target)
        {
            SetHeadUp(false);
            RotateTo(Target.transform.position);
            SetWalk(true);
            WalkTo(Target.transform.position, FaceCamera);
        }
    }
    private void FaceCamera()
    {
        RotateTo(Vector3.back * 1000);
        SetHeadUp(true);
        
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
