using Newtonsoft.Json;
using UnityEngine;

public class SaveSystem {
    private const string DataKey = "SaveSystem";
    public bool SaveIsLoad = false;

    public void Save(GameData gameData) {
        string stringData = JsonConvert.SerializeObject(gameData);
        PlayerPrefs.SetString(DataKey, stringData);
    }

    public GameData LoadFromDevice() {
        string stringData = PlayerPrefs.GetString(DataKey);
        return JsonConvert.DeserializeObject<GameData>(stringData);
    }
}
