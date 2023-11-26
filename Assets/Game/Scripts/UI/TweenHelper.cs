using DG.Tweening;
using UnityEngine;

public static class TweenHelper {
    public static void CheckSequences() {
        var playingTweens = DOTween.PlayingTweens();
        if ((playingTweens == null) || (playingTweens.Count <= 0)) {
            return;
        }

        DOTween.ClearCachedTweens();
        DOTween.Clear(true);
    }

    public static Sequence CreateSequence(object owner) {
        var seq = DOTween.Sequence();
        return seq;
    }

    public static Sequence ResetSequence(Sequence seq, bool complete = true, bool withCallbacks = false) {
        if (seq == null) {
            return null;
        }

        seq.SetAutoKill(false);
        if (complete) {
            seq.Complete(withCallbacks);
        }

        if (seq.IsActive()) {
            seq.Kill();
        }

        return null;
    }

    public static Tween ResetTween(Tween tween, bool complete = true, bool withCallbacks = false) {
        if (tween == null) {
            return null;
        }

        tween.SetAutoKill(false);
        if (complete) {
            tween.Complete(withCallbacks);
        }

        tween.Kill();
        return null;
    }

    public static Sequence ReplaceSequence(object owner, Sequence seq, bool complete = true, bool withCallbacks = false) {
        ResetSequence(seq, complete, withCallbacks);
        return CreateSequence(owner);
    }

    public static void SendItemTo(Sequence seq, Transform moveItem, Transform endTrans, Transform midTrans, float speedScale = 1.0f) {
        if ((seq == null) || (endTrans == null) || (moveItem == null)) {
            return;
        }
        var t = seq.Duration();
        if (midTrans) {
            var path = new[] { moveItem.position, midTrans.position, endTrans.position };
            seq.Append(moveItem.DOPath(path, 0.5f * speedScale, PathType.CatmullRom));
        }
        else {
            seq.Append(moveItem.DOMove(endTrans.position, 0.5f * speedScale));
        }
        seq.Insert(t, moveItem.DOScale(endTrans.localScale, 0.5f * speedScale).SetEase(Ease.OutElastic));
    }

    public static void SendItemToInsert(Sequence seq, Transform moveItem, Transform endTrans, Transform midTrans, float delay) {
        if (midTrans) {
            var path = new[] { moveItem.position, midTrans.position, endTrans.position };
            seq.Insert(delay, moveItem.DOPath(path, 0.5f, PathType.CatmullRom));
        }
        else {
            seq.Insert(delay, moveItem.DOMove(endTrans.position, 0.5f));
        }
    }

    public static void DoRewardEffect(GameObject effect, int reward, float height = 200f) {
        var efTrans = effect.transform;
        var efGroup = effect.GetComponent<CanvasGroup>();

        efTrans.localScale = Vector3.one;
        if (efGroup) {
            efGroup.alpha = 1.0f;
        }

        effect.SetActive(true);
        var startPos = RectUtils.GetLocalPosition(efTrans);
        efTrans.DOLocalMove(startPos + new Vector3(0, height, 0), 1.0f);

        var dotSeq = CreateSequence(effect);
        dotSeq.Append(efTrans.DOShakeScale(1.0f, 0.2f));
        if (efGroup) {
            dotSeq.Append(efGroup.DOFade(0.0f, 1.0f));
        }
        dotSeq.AppendCallback(
            () =>
            {
                effect.SetActive(false);
            }
        );
    }

    public static Tween DoVolume(this AudioSource source, float endValue, float duration) {
        return DOTween.To(() => source.volume, volume => source.volume = volume, endValue, duration);
    }
}
