using System;
using UnityEngine;
using UnityEngine.UI;

namespace RicKit.UI
{
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup), typeof(GraphicRaycaster))]
    public abstract class AbstractUIPanel : MonoBehaviour
    {
        public int SortOrder { get; private set; }
        private bool IsShow { get; set; }
        public bool CanInteract => IsShow && CanvasGroup.interactable;
        protected CanvasGroup CanvasGroup { get; private set; }
        protected RectTransform CanvasRect { get; private set; }
        private Canvas Canvas { get; set; }
        public ShowType ShowType { get; set; }
        public virtual bool DontDestroyOnClear => false;
        public virtual bool ShowHideSound => true;
        protected static UIManager UI => UIManager.I;

        protected virtual void Awake()
        {
            Canvas = GetComponent<Canvas>();
            Canvas.overrideSorting = true;
            Canvas.sortingLayerName = "UI";
            CanvasGroup = GetComponent<CanvasGroup>();
            CanvasRect = Canvas.GetComponent<RectTransform>();
        }

        public void OnShow()
        {
            IsShow = true;
            gameObject.SetActive(true);
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.interactable = false;
            OnAnimationIn();
        }

        public void OnHide(Action callback = null)
        {
            IsShow = false;
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.interactable = false;
            OnAnimationOut(callback);
        }

        public void UpdateData<T>(Action<T> onUpdateData) where T : AbstractUIPanel
        {
            onUpdateData?.Invoke((T)this);
        }

        public virtual void BeforeShow()
        {
        }

        public abstract void OnESCClick();
        protected abstract void OnAnimationIn();

        protected abstract void OnAnimationOut(Action callback);

        protected virtual void OnAnimationInEnd()
        {
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.interactable = true;
        }

        protected virtual void OnAnimationOutEnd(Action callback)
        {
            callback?.Invoke();
            gameObject.SetActive(false);
        }

        public void SetSortOrder(int order)
        {
            SortOrder = order;
            Canvas.overrideSorting = true;
            Canvas.sortingOrder = order;
        }
    }
}