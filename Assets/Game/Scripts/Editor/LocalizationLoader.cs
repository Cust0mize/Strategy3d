using System.Collections.Generic;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Newtonsoft.Json.Linq;
using Google.Apis.Services;
using Newtonsoft.Json;
using UnityEngine;
using System.Text;
using UnityEditor;
using System.IO;

public class LocalizationLoader : EditorWindow {
    private string _sheetName = "Locale";                                     //Название таблицы и страницы, лучше сделать одинаковым
    private string _sheetID = "1hz8sOJL94EoisdKjGJ3-Cc4bjTftX8Cgjgwl17jNgJk"; //ИД таблицы, находится в самой таблице после d/ и до edit 
    private SheetsService _service;

    [MenuItem("Custom/Localization")]
    private static void ShowWindow() {
        GetWindow(typeof(LocalizationLoader));
    }

    private void OnGUI() {
        _sheetName = EditorGUILayout.TextField("Sheet Name", _sheetName);
        _sheetID = EditorGUILayout.TextField("Sheet ID", _sheetID);
        if (!string.IsNullOrEmpty(_sheetName)) {
            ButtonClick();
        }
    }

    private void LoadSheets() {
        string jsonKey = Resources.Load<TextAsset>("SheetKey").ToString();
        var jsonData = (JObject)JsonConvert.DeserializeObject(jsonKey);
        if (jsonData != null) {
            ServiceAccountCredential.Initializer initializer = new ServiceAccountCredential.Initializer(jsonData["client_email"].Value<string>());
            ServiceAccountCredential credential = new ServiceAccountCredential(initializer.FromPrivateKey(jsonData["private_key"].Value<string>()));
            _service = new SheetsService(new BaseClientService.Initializer() {
                HttpClientInitializer = credential
            });
        }

        var sheet = GetSheetRange($"{_sheetName}!A1:Z100000");
        for (int i = 1; i < sheet[0].Count; i++) {
            WriteToCSV(sheet, i);
        }
    }

    private IList<IList<object>> GetSheetRange(string sheetNameAndRange) {
        SpreadsheetsResource.ValuesResource.GetRequest request = _service.Spreadsheets.Values.Get(_sheetID, sheetNameAndRange);
        StringBuilder sb = new StringBuilder();
        ValueRange response = request.Execute();
        IList<IList<object>> values = response.Values;

        if (values != null && values.Count > 0) {
            foreach (var row in values) {
                foreach (var cell in row) {
                    sb.Append(cell + " ");
                }
            }
            sb.Clear();
        }
        else {
            Debug.Log("No data");
        }
        return values;
    }

    private void ButtonClick() {
        if (GUILayout.Button("Load localization")) {
            LoadSheets();
            AssetDatabase.SaveAssets();
        }
    }

    private void WriteToCSV(IList<IList<object>> values, int i) {
        string path = Path.Combine("Assets/Resources/Configs", $"{values[0][i]}.csv");
        Directory.CreateDirectory(Path.GetDirectoryName(path));

        using (StreamWriter writer = new StreamWriter(path, false)) {
            foreach (var row in values) {
                var line = new StringBuilder();
                for (int j = 0; j < row.Count; j++) {
                    if (j == 0 || j == i) {
                        if (line.Length > 0) {
                            line.Append(",");
                        }
                        string appendText = LocalizationLoaderParceService.ReplaceText(row[j].ToString());
                        line.Append(appendText);
                    }
                }
                writer.WriteLine(line.ToString());
            }
        }
    }
}
