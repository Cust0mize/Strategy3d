using DG.Tweening;
using UnityEngine;
using Zenject;

public class UIPanel : MonoBehaviour {
    [field: SerializeField] protected CanvasGroup CanvasGroup { get; private set; }
    public RectTransform CachedTransform { get; private set; }
    private WindowShowHideAnimation _windowShowHideAnimation;
    [SerializeField] private StartAnimType _startAnimType;
    [SerializeField] private EndAnimType _endAnimType;

    private ScreenBlurController _screenBlurController;
    protected UIService UIService { get; private set; }

    [Inject]
    private void Construct(
    WindowShowHideAnimation windowShowHideAnimation,
    //ScreenBlurController screenBlurController,
    UIService uiService
    ) {
        _windowShowHideAnimation = windowShowHideAnimation;
        //_screenBlurController = screenBlurController;
        UIService = uiService;
    }

    public virtual void Show() {
        if (CachedTransform == null) {
            CachedTransform = transform as RectTransform;
        }
        _windowShowHideAnimation.GetShowSequence(_startAnimType, CanvasGroup, CachedTransform);
        gameObject.SetActive(true);
        UIService.HideUIElements();
        //_screenBlurController.OnShowBlur(false, false, true);
    }

    public virtual void Hide() {
        _windowShowHideAnimation.GetHideSequence(_endAnimType, CanvasGroup, CachedTransform).OnComplete(() => gameObject.SetActive(false));
        //_screenBlurController.OnShowBlur(false, false, false);
        UIService.ShowUIElements();
    }
}
