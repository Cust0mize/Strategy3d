using UnityEditor.SceneManagement;
using UnityEditor;

public static partial class SceneMenu {
    public const string LOADING_SCENE_PATH = "Assets/Game/Scenes/EntryPointStarter.unity";

    public const string GAME_SCENE_PATH = "Assets/Game/Scenes/Game.unity";

    [MenuItem("Custom/Scene/Open Loading Scene")]
    private static void OpenLoadingScene() {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
            EditorSceneManager.OpenScene(LOADING_SCENE_PATH);
        }
    }

    [MenuItem("Custom/Scene/Open Game Scene")]
    private static void OpenGameScene() {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
            EditorSceneManager.OpenScene(GAME_SCENE_PATH);
        }
    }
}
