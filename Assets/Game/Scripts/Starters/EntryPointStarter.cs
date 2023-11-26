using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Starters {
    public class EntryPointStarter : MonoBehaviour {
        private LocalizationService _localizationService;
        private SceneLoaderService _sceneLoaderService;
        private ConfigService _configService;
        private SaveSystem _saveSystem;
        private GameData _gameData;

        [Inject]
        private void Construct(
        LocalizationService localizationService,
        SceneLoaderService sceneLoaderService,
        ConfigService configService,
        SaveSystem saveSystem,
        GameData gameData
        ) {
            _localizationService = localizationService;
            _sceneLoaderService = sceneLoaderService;
            _configService = configService;
            _saveSystem = saveSystem;
            _gameData = gameData;
        }

        private void Start() {
            _gameData.Init(_saveSystem);
            _gameData.SetValue(_saveSystem.LoadFromDevice());
            _configService.LoadConfigs();
            _localizationService.LoadLanguage();
            _localizationService.TryStartLocalization(_localizationService.CurrentLanguage);
            _sceneLoaderService.LoadScene(SceneName.Game);
        }
    }
}