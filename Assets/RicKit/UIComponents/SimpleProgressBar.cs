using DG.Tweening;
using UnityEngine;

namespace RicKit.UIComponents
{
    public class SimpleProgressBar : MonoBehaviour
    {
        [SerializeField] protected RectTransform bar;
        protected Sequence changeSeq, showSeq;
        private CanvasGroup cg;
        private float maxWidth;
        private float height;
        private void Awake()
        {
            cg = GetComponent<CanvasGroup>();
            maxWidth = bar.sizeDelta.x;
            height = bar.sizeDelta.y;
        }

        public void SetProgress(float progress)
        {
            bar.sizeDelta = new Vector2(maxWidth * progress, height);
        }

        public Tween Show()
        {
            showSeq?.Kill();
            showSeq = DOTween.Sequence().Append(cg.DOFade(1, 0.2f));
            return showSeq;
        }

        public void ShowImmediate()
        {
            showSeq?.Kill();
            cg.alpha = 1;
        }

        public Tween Hide()
        {
            showSeq?.Kill();
            showSeq = DOTween.Sequence().Append(cg.DOFade(0, 0.2f));
            return showSeq;
        }

        public void HideImmediate()
        {
            showSeq?.Kill();
            cg.alpha = 0;
        }

        public void ChangeProgress(float progress, float duration)
        {
            changeSeq?.Kill();
            changeSeq = DOTween.Sequence().Append(bar.DOSizeDelta(new Vector2(maxWidth * progress, height), duration));
        }
    }
}