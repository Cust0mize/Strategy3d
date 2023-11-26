using Assets.Game.Scripts.Configs;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

public class ConfigService {
    public Dictionary<Locale, List<LocaleConfig>> LocaleConfigs { get; private set; } = new Dictionary<Locale, List<LocaleConfig>>();
    private ResourceLoaderService _resourceLoader;

    [Inject]
    public ConfigService(
    ResourceLoaderService resourceLoader
    ) {
        _resourceLoader = resourceLoader;
    }

    public void LoadConfigs() {
        LoadLocale();
    }

    private void LoadLocale() {
        foreach (Locale locale in Enum.GetValues(typeof(Locale))) {
            if (locale == Locale.None) {
                continue;
            }
            LocaleConfigs.Add(locale, LoadLocaleConfig(locale));
        }
    }

    private List<LocaleConfig> LoadLocaleConfig(Locale locale) {
        TextAsset fileRaw = _resourceLoader.GetConfigFile(locale.ToString());
        string[,] fileGrid = CsvParserService.SplitCsvGrid(fileRaw.text);

        List<LocaleConfig> configs = new List<LocaleConfig>();
        for (int y = 1; y < fileGrid.GetLength(1); y++) {
            if (string.IsNullOrEmpty(fileGrid[1, y])) {
                continue;
            }

            string ID = fileGrid[0, y];
            string Text = fileGrid[1, y];

            configs.Add(new LocaleConfig {
                ID = ID,
                Text = Text,
            });
        }
        return configs;
    }
}

//float timeSpawn = float.Parse(fileGrid[0, y], CultureInfo.InvariantCulture);
//float moveTimeFromStartPointToTarget = float.Parse(fileGrid[1, y], CultureInfo.InvariantCulture);
//float moveTimeFromFirstTargetToAnyTarget = float.Parse(fileGrid[2, y], CultureInfo.InvariantCulture);
//float moveTimeFromCassToCar = float.Parse(fileGrid[3, y], CultureInfo.InvariantCulture);
//int carPrice = int.Parse(fileGrid[4, y], CultureInfo.InvariantCulture);