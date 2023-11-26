using UnityEngine;
using UnityEditor;
using Zenject;

public class UntitledEditorWindow : ZenjectEditorWindow {
    [MenuItem("Window/UntitledEditorWindow")]
    public static UntitledEditorWindow GetOrCreateWindow() {
        var window = GetWindow<UntitledEditorWindow>();
        window.titleContent = new GUIContent("UntitledEditorWindow");
        return window;
    }

    public override void InstallBindings() {
        // TODO
    }
}