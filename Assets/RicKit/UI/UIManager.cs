using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using RicKit.Comon;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RicKit.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] private CanvasGroup blockerCg;
        public Transform defaultRoot;
        private readonly IPanelLoader panelLoader = new DefaultPanelLoader();
        private readonly Stack<AbstractUIPanel> showFormStack = new Stack<AbstractUIPanel>();
        private readonly List<AbstractUIPanel> uiFormsList = new List<AbstractUIPanel>();
        private Canvas canvas;
        private int BlockCount { get; set; }
        private AbstractUIPanel CurrentAbstractUIPanel { get; set; }
        public RectTransform RT { get; private set; }
        public Camera UICamera { get; private set; }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && CurrentAbstractUIPanel && CurrentAbstractUIPanel.CanInteract)
                CurrentAbstractUIPanel.OnESCClick();
        }

        public void ClearAll()
        {
            foreach (var uIForm in uiFormsList.Where(uIForm => !uIForm.DontDestroyOnClear))
                Destroy(uIForm.gameObject);
            uiFormsList.Clear();
            showFormStack.Clear();
            StopAllCoroutines();
            CurrentAbstractUIPanel = null;
            ResetLockInput();
        }

        protected override void GetAwake()
        {
            DOTween.SetTweensCapacity(500, 125);
            UICamera = GetComponentInChildren<Camera>();
            canvas = GetComponentInChildren<Canvas>();
            RT = canvas.GetComponent<RectTransform>();
            DontDestroyOnLoad(gameObject);
            var eventSystem = FindObjectOfType<EventSystem>();
            eventSystem = eventSystem
                ? eventSystem
                : new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule))
                    .GetComponent<EventSystem>();
            DontDestroyOnLoad(eventSystem);
        }

        /// <summary>
        ///     隐藏当前界面，打开目标界面
        /// </summary>
        /// <typeparam name="T">UIForm</typeparam>
        /// <param name="eUIType">弹窗还是全屏</param>
        /// <param name="onInit"></param>
        public T ShowUI<T>(ShowType eUIType = default, Action<T> onInit = null) where T : AbstractUIPanel
        {
            LockInput(true);
            var sortOrder = 1;
            if (CurrentAbstractUIPanel) sortOrder = CurrentAbstractUIPanel.SortOrder + 5;
            var form = GetUI<T>();
            if (!form)
                form = NewUI<T>();
            form.gameObject.SetActive(false);
            form.ShowType = eUIType;
            onInit?.Invoke(form);
            form.SetSortOrder(sortOrder);
            showFormStack.Push(form);
            if (!uiFormsList.Contains(form)) uiFormsList.Add(form);

            if (CurrentAbstractUIPanel)
            {
                if (form.ShowType == ShowType.HideAndShow)
                {
                    CurrentAbstractUIPanel.OnHide(() =>
                    {
                        form.BeforeShow();
                        form.OnShow();
                    });
                }
                else
                {
                    form.BeforeShow();
                    form.OnShow();
                }
            }
            else
            {
                form.BeforeShow();
                form.OnShow();
            }

            CurrentAbstractUIPanel = form;
            LockInput(false);
            return form;
        }

        public void ShowUIUnmanagable<T>(Action<T> onInit = null) where T : AbstractUIPanel
        {
            LockInput(true);
            const int sortOrder = 900;
            var form = GetUI<T>();
            if (!form)
                form = NewUI<T>();
            form.gameObject.SetActive(false);
            form.ShowType = ShowType.JustShow;
            onInit?.Invoke(form);
            form.SetSortOrder(sortOrder);
            if (!uiFormsList.Contains(form)) uiFormsList.Add(form);
            form.BeforeShow();
            form.OnShow();
            LockInput(false);
        }

        public void HideSecond()
        {
            if (showFormStack.Count < 2) return;
            var form = showFormStack.Pop();
            var formToHide = showFormStack.Pop();
            showFormStack.Push(form);
            formToHide.OnHide();
        }

        private void LockInput(bool on)
        {
            BlockCount += on ? 1 : -1;
            if (BlockCount > 0 && !on) return;
#if UNITY_EDITOR
            blockerCg.name = on ? "blocker on" : "blocker off";
#endif
            blockerCg.blocksRaycasts = on;
        }

        private void ResetLockInput()
        {
            BlockCount = 0;
            blockerCg.blocksRaycasts = false;
        } // ReSharper disable Unity.PerformanceAnalysis
        public void DelayToDo(float time, Action callback, bool blockRaycast = true)
        {
            StartCoroutine(DelayCo(time, callback, blockRaycast));
        }

        private IEnumerator DelayCo(float time, Action callback, bool blockRaycast)
        {
            if (blockRaycast)
            {
                LockInput(true);
                yield return new WaitForSeconds(time);
                LockInput(false);
                callback();
            }
            else
            {
                yield return new WaitForSeconds(time);
                callback();
            }
        }

        public void Back()
        {
            var form = showFormStack.Pop();
            if (showFormStack.Count > 0)
            {
                form.OnHide(() =>
                {
                    CurrentAbstractUIPanel = showFormStack.Peek();
                    CurrentAbstractUIPanel.BeforeShow();
                    if (form.ShowType == ShowType.HideAndShow) CurrentAbstractUIPanel.OnShow();
                });
            }
            else
            {
                CurrentAbstractUIPanel = null;
                form.OnHide();
            }
        }

        public void BackThenShow<T>(Action<T> onInit = null) where T : AbstractUIPanel
        {
            var form = showFormStack.Pop();
            if (showFormStack.Count > 0)
                if (form.ShowType == ShowType.JustShow)
                    showFormStack.Peek().OnHide();
            CurrentAbstractUIPanel = null;
            form.OnHide(() => ShowUI(onInit: onInit));
        }

        public void HideAllExcept<T>() where T : AbstractUIPanel
        {
            while (showFormStack.Count > 0)
            {
                var form = showFormStack.Peek();
                if (form is T)
                {
                    if (!form.isActiveAndEnabled) form.OnShow();

                    CurrentAbstractUIPanel = form;
                    return;
                }

                showFormStack.Pop();
                form.OnHide();
            }
        }

        public T GetUI<T>() where T : AbstractUIPanel
        {
            return uiFormsList.Where(form => form is T).Cast<T>().FirstOrDefault();
        }

        private T NewUI<T>() where T : AbstractUIPanel
        {
            var go = Instantiate(panelLoader.LoadPrefab(typeof(T).Name), defaultRoot);
            go.TryGetComponent(out T form);
            return form;
        }
    }

    public interface IPanelLoader
    {
        GameObject LoadPrefab(string name);
    }

    public class DefaultPanelLoader : IPanelLoader
    {
        private const string PrefabPath = "UIPanels/";

        public GameObject LoadPrefab(string name)
        {
            return Resources.Load<GameObject>(PrefabPath + name);
        }
    }

    public enum ShowType
    {
        HideAndShow,
        JustShow
    }
}