using System;

[Serializable]
public class GameData {
    private SaveSystem _saveSystem;

    #region Properties
    private Locale _currentLocale;
    public Locale CurrentLocale {
        get { return _currentLocale; }
        set {
            _currentLocale = value;
            Save();
        }
    }

    #endregion

    public void Init(SaveSystem saveSystem) {
        _saveSystem = saveSystem;
    }

    public void Save() {
        if (_saveSystem != null) {
            _saveSystem.Save(this);
        }
    }

    public void SetValue(GameData gameData) {
        if (gameData == null) {
            return;
        }
        CurrentLocale = gameData.CurrentLocale;
    }

    public void ResetGame() {

    }
}
