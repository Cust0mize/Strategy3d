using DG.Tweening;
using UnityEngine;

public abstract class UIElement : MonoBehaviour {
    [field: SerializeField] public bool IHidenElement { get; private set; } = true;
    [SerializeField] private Vector3 _hidenOffset;

    protected Sequence Sequence;
    protected RectTransform RectTransform {
        get {
            if (!_rectTransform) {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }
    private RectTransform _rectTransform;
    public Vector3 StartLocalPosition { get; protected set; } = Vector3.zero;
    private float _hideProgress;
    public bool IsShown { get; protected set; } = true;
    protected const float TweenDuration = 0.5f;
    private Vector3 LocalHideOffset => _hideProgress * _hidenOffset;

    public abstract void Init();

    public virtual void Hide(bool force = false) {
        if (force) {
            Sequence = TweenHelper.ResetSequence(Sequence);
            RectTransform.anchoredPosition = StartLocalPosition + _hidenOffset;
            _hideProgress = 1f;
            IsShown = false;
            return;
        }

        if (!IsShown) {
            return;
        }

        Sequence = TweenHelper.ReplaceSequence(gameObject, Sequence, false);
        PlayMoveAnimation(true);
        IsShown = false;
    }

    public virtual void Show(bool force = false) {
        if (force) {
            Sequence = TweenHelper.ResetSequence(Sequence);
            RectTransform.anchoredPosition = StartLocalPosition;
            _hideProgress = 0f;
            IsShown = true;
            return;
        }

        if (IsShown) {
            return;
        }

        Sequence = TweenHelper.ReplaceSequence(gameObject, Sequence, false);
        PlayMoveAnimation(false);
        IsShown = true;
    }


    private void PlayMoveAnimation(bool hide) {
        var endValue = hide ? 1f : 0f;
        var duration = TweenDuration * Mathf.Abs(endValue - _hideProgress);

        Sequence.Append(DOTween.To(() => _hideProgress, val =>
        {
            _hideProgress = val;
            RectTransform.anchoredPosition = StartLocalPosition + LocalHideOffset;
        }, endValue, duration));
    }
}