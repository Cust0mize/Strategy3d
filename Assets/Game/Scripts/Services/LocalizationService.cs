using Assets.Game.Scripts.Configs;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LocalizationService {
    private readonly Dictionary<string, string> _localization = new Dictionary<string, string>();
    private readonly ConfigService _configService;
    private readonly GameData _gameData;

    private bool _localizationLoading;
    private Locale _currentLocale;

    public Locale CurrentLanguage => _currentLocale;

    public delegate void LocalizationLoadDelegate(Locale language);

    [Inject]
    public LocalizationService(GameData gameData, ConfigService configService) {
        _configService = configService;
        _gameData = gameData;
    }

    public event LocalizationLoadDelegate OnLocalizationLoad;

    public string GetText(string id) {
        return _localization.ContainsKey(id) ? _localization[id] : id;
    }

    public void LoadLanguage() {
        _currentLocale = _gameData.CurrentLocale;
        Debug.Log("Тут грузить язык");

        if (_currentLocale == Locale.None) {
            switch (Application.systemLanguage) {
                case SystemLanguage.Russian:
                    _currentLocale = Locale.RU;
                    _gameData.CurrentLocale = Locale.RU;
                    break;
                default:
                    _currentLocale = Locale.EN;
                    _gameData.CurrentLocale = Locale.EN;
                    break;
            }
        }
    }

    public void TryStartLocalization(Locale locale) {
        if (_localizationLoading) {
            return;
        }

        _localizationLoading = true;
        _currentLocale = locale;
        _gameData.CurrentLocale = locale;
        LoadLocalization(locale);
    }

    private void LoadLocalization(Locale currentLocale) {
        _localization.Clear();
        List<LocaleConfig> currentConfigs = _configService.LocaleConfigs[currentLocale];

        foreach (var config in currentConfigs) {
            string text = CsvParserService.TextReplacer(config.Text);
            _localization.Add(config.ID, text);
        }

        _gameData.CurrentLocale = currentLocale;
        _localizationLoading = false;
        OnLocalizationLoad?.Invoke(currentLocale);
    }
}