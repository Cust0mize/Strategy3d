using DG.Tweening;
using UnityEngine;
using ScreenBlur;

[RequireComponent(typeof(SuperBlur))]
public sealed class ScreenBlurController : MonoBehaviour {
    public Color AdditiveColor = Color.clear;
    public Color Tint = Color.white;

    private Sequence _sequence;
    private SuperBlur _blur;

    void Start() {
        _blur = GetComponent<SuperBlur>();
        _blur.AdditiveColor = AdditiveColor;
        _blur.AdditiveColor.a = 0f;
    }

    void OnDestroy() {
        _sequence = TweenHelper.ResetSequence(_sequence);
    }

    public void OnShowBlur(bool force, bool applyColorCorrection, bool enable) {
        _sequence = TweenHelper.ReplaceSequence(this, _sequence);

        if (force) {
            _sequence.Append(DOTween.To(() => _blur.interpolation, x => { _blur.interpolation = x; }, enable ? 1f : 0f, 0.1f));
            if (applyColorCorrection) {
                _sequence.Insert(0f, DOTween.To(() => _blur.Tint, x => { _blur.Tint = x; }, enable ? Tint : Color.white, 0.09f));
                _sequence.Insert(0f, DOTween.To(() => _blur.AdditiveColor.a, x => { _blur.AdditiveColor.a = x; }, enable ? AdditiveColor.a : 0f, 0.08f));
            }

            _blur.enabled = enable;
            _sequence = TweenHelper.ResetSequence(_sequence);
            return;
        }

        _blur.enabled = enable || _blur.enabled;
        _sequence.Append(DOTween.To(() => _blur.interpolation, x => { _blur.interpolation = x; }, enable ? 1f : 0f, 0.5f));
        if (applyColorCorrection) {
            _sequence.Insert(0f, DOTween.To(() => _blur.Tint, x => { _blur.Tint = x; }, enable ? Tint : Color.white, 0.48f));
            _sequence.Insert(0f, DOTween.To(() => _blur.AdditiveColor.a, x => { _blur.AdditiveColor.a = x; }, enable ? AdditiveColor.a : 0f, 0.4f));
        }

        if (!enable) {
            _sequence.AppendCallback(() => { _blur.enabled = false; });
        }
    }
}
