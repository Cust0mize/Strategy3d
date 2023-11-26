using DG.Tweening;
using UnityEngine;

public sealed class WindowShowHideAnimation {
    private Vector2 _initPos = Vector2.zero;

    public Sequence GetShowSequence(StartAnimType startAnimType, CanvasGroup canvasGroup, RectTransform cachedTransform) {
        switch (startAnimType) {
            case StartAnimType.ScaleUp: return GetShowSequenceScaled(canvasGroup, cachedTransform);
            case StartAnimType.FromDown: return GetShowSequenceFrom(Vector2.down, canvasGroup, cachedTransform);
            case StartAnimType.FromTop: return GetShowSequenceFrom(Vector2.up, canvasGroup, cachedTransform);
            case StartAnimType.FromLeft: return GetShowSequenceFrom(Vector2.left, canvasGroup, cachedTransform);
            case StartAnimType.FromRight: return GetShowSequenceFrom(Vector2.right, canvasGroup, cachedTransform);
            case StartAnimType.RotateScaleUp: return GetShowSequenceRotateScaled(cachedTransform);
            case StartAnimType.EventStartShow: return GetShowSequenceEventStartShow(canvasGroup, cachedTransform);
            case StartAnimType.FadeUp: return GetShowSequenceFadeShow(canvasGroup);
            case StartAnimType.NoAnim: return GetShowSequenceNoAnim(canvasGroup);
            default: return TweenHelper.CreateSequence(this);
        }
    }

    public Sequence GetHideSequence(EndAnimType endAnimType, CanvasGroup canvasGroup, RectTransform cachedTransform) {
        switch (endAnimType) {
            case EndAnimType.ScaleDown: return GetHideSequenceScaled(cachedTransform);
            case EndAnimType.ToDown: return GetHideSequenceTo(Vector2.down, canvasGroup, cachedTransform);
            case EndAnimType.ToTop: return GetHideSequenceTo(Vector2.up, canvasGroup, cachedTransform);
            case EndAnimType.ToLeft: return GetHideSequenceTo(Vector2.left, canvasGroup, cachedTransform);
            case EndAnimType.ToRight: return GetHideSequenceTo(Vector2.right, canvasGroup, cachedTransform);
            case EndAnimType.FadeDown: return GetHideSequenceFadeHide(canvasGroup);
            default: return TweenHelper.CreateSequence(this);
        }
    }

    public float GetShowCallbackTime(StartAnimType startAnimType) {
        switch (startAnimType) {
            case StartAnimType.NoAnim: return 0;
            case StartAnimType.ScaleUp: return ShowFadeTime;
            case StartAnimType.FromTop: return ShowMoveTime / 2;
            case StartAnimType.RotateScaleUp: return ShowRotateTime;
            default: return ShowMoveTime;
        }
    }

    #region Show Animation
    const float ShowFadeTime = 0.3f;
    const float ShowMoveTime = 0.25f;
    const float ShowRotateTime = 0.25f;
    const float ShowOffset = 3000;
    const float ShowShakePosTime = 0.1f;
    const float ShowShakeScaleTime = 0.25f;
    const float ShowScaleDown = 0.004f;
    const float ShakeForce = 20;
    const float ShowScaleUpTime = 0.35f;
    const float AppearFadeTime = 0.25f;
    const float HideDisappearTime = ShowFadeTime;

    private Sequence GetShowSequenceNoAnim(CanvasGroup canvasGroup) {
        if (canvasGroup) {
            canvasGroup.alpha = 1;
        }
        return TweenHelper.CreateSequence(this);
    }

    private Sequence GetShowSequenceScaled(CanvasGroup canvasGroup, RectTransform cachedTransform) {
        cachedTransform.localScale = Vector3.zero;
        var sequence = TweenHelper.CreateSequence(this)
            .Append(cachedTransform.DOScale(1.1f, ShowScaleUpTime * 0.7f))
            .Append(cachedTransform.DOScale(1f, ShowScaleUpTime * 0.3f));
        if (canvasGroup) {
            canvasGroup.alpha = 0;
            sequence.Insert(0, canvasGroup.DOFade(1, ShowFadeTime));
        }
        return sequence;
    }

    private Sequence GetShowSequenceFadeShow(CanvasGroup canvasGroup) {
        var sequence = TweenHelper.CreateSequence(this);
        canvasGroup.alpha = 0;
        sequence.Append(canvasGroup.DOFade(1, AppearFadeTime));
        return sequence;
    }

    private Sequence GetHideSequenceFadeHide(CanvasGroup canvasGroup) {
        var sequence = TweenHelper.CreateSequence(this);
        canvasGroup.alpha = 1;
        sequence.Append(canvasGroup.DOFade(0f, HideDisappearTime));
        return sequence;
    }

    private Sequence GetShowSequenceFrom(Vector2 dir, CanvasGroup canvasGroup, RectTransform cachedtransform) {
        var sequence = TweenHelper.CreateSequence(this);
        if (canvasGroup) {
            canvasGroup.alpha = 0.5f;
            sequence.Append(canvasGroup.DOFade(1, ShowFadeTime));
        }
        cachedtransform.localScale = Vector3.one;
        var currentPosition = cachedtransform.anchoredPosition;
        cachedtransform.anchoredPosition = currentPosition + dir * ShowOffset;
        sequence.Insert(0, cachedtransform.DOAnchorPos(currentPosition - dir * ShakeForce, ShowMoveTime).SetEase(Ease.OutQuad));
        sequence.Append(cachedtransform.DOAnchorPos(currentPosition, ShowShakePosTime).SetEase(Ease.InOutQuad));
        return sequence;
    }

    private Sequence GetShowSequenceEventStartShow(CanvasGroup canvasGroup, RectTransform cachedTransform) {
        var dir = Vector2.left;
        var sequence = TweenHelper.CreateSequence(this);
        if (canvasGroup) {
            canvasGroup.alpha = 0;
            sequence.Append(canvasGroup.DOFade(1, ShowFadeTime));
        }
        cachedTransform.localScale = Vector3.one;
        var currentPosition = cachedTransform.anchoredPosition;
        cachedTransform.anchoredPosition = currentPosition + dir * ShowOffset;
        sequence.Insert(0.05f, cachedTransform.DOAnchorPos(currentPosition, 0.5f));
        sequence.Append(cachedTransform.DOShakeAnchorPos(0.3f, -dir * 100, 5, 0, true));
        sequence.Insert(0.6f, cachedTransform.DOPunchScale(dir * ShowScaleDown * 1, ShowShakeScaleTime, 3).SetEase(Ease.InOutBounce));
        return sequence;
    }

    private Sequence GetShowSequenceRotateScaled(RectTransform cachedTransform) {
        var sequence = TweenHelper.CreateSequence(this);
        cachedTransform.localScale = Vector3.zero;
        sequence.Append(cachedTransform.DOScale(Vector3.one, ShowRotateTime));
        sequence.Join(cachedTransform.DORotate(Vector3.forward * 360, ShowRotateTime, RotateMode.FastBeyond360));
        return sequence;
    }
    #endregion

    #region Hide Animation
    const float HideFadeDelay = 0.136f;
    const float HideFadeTime = 0.255f;
    const float HideScaleUp = 1.2f;
    const float HideScaleUpTime = 0.068f;
    const float HideScaleDownTime = 0.34f;
    const float HideMoveFadeTime = 1.15f;
    const float HideOffset = 3000;

    private Sequence GetHideSequenceScaled(RectTransform cachedTransform) {
        var sequence = TweenHelper.CreateSequence(this).Append(cachedTransform.DOScale(0, HideScaleDownTime));
        sequence.AppendCallback(() =>
        {
            cachedTransform.localScale = Vector3.one;
        });
        return sequence;
    }

    private Sequence GetHideSequenceTo(Vector2 dir, CanvasGroup canvasGroup, RectTransform cachedTransform) {
        var sequence = TweenHelper.CreateSequence(this);
        cachedTransform.localScale = Vector3.one;
        var nextAnchoredPosition = cachedTransform.anchoredPosition + dir * HideOffset;
        sequence.Append(cachedTransform.DOAnchorPos(nextAnchoredPosition, ShowMoveTime).SetEase(Ease.InSine));
        if (canvasGroup) {
            sequence.Join(canvasGroup.DOFade(0.5f, ShowMoveTime));
        }
        sequence.AppendCallback(() =>
        {
            cachedTransform.anchoredPosition = _initPos;
        });
        return sequence;
    }
    #endregion
}

public enum StartAnimType {
    NoAnim,
    ScaleUp,
    FromLeft,
    FromRight,
    FromTop,
    FromDown,
    RotateScaleUp,
    EventStartShow,
    FadeUp,
}

public enum EndAnimType {
    NoAnim,
    ScaleDown,
    ToLeft,
    ToRight,
    ToTop,
    ToDown,
    FadeDown,
}