using UnityEngine;
using Zenject;
using TMPro;

public class StaticLocalization : MonoBehaviour {
    [SerializeField] private string _key;

    private LocalizationService _localizationManager;
    private TextMeshProUGUI _text;

    private TextMeshProUGUI Text {
        get {
            if (_text == null) {
                _text = GetComponent<TextMeshProUGUI>();
            }
            return _text;
        }
    }

    [Inject]
    public void Construct(LocalizationService localizationManager) {
        _localizationManager = localizationManager;
    }

    private void OnEnable() {
        if (_localizationManager != null) {
            _localizationManager.OnLocalizationLoad += OnLocalizationLoad;

            UpdateText();
        }
    }

    private void OnDisable() {
        if (_localizationManager != null) {
            _localizationManager.OnLocalizationLoad -= OnLocalizationLoad;
        }
    }

    public void UpdateText() {
        if (Text != null) {
            Text.text = _localizationManager.GetText(_key);
        }
    }

    private void OnLocalizationLoad(Locale locale) {
        UpdateText();
    }
}