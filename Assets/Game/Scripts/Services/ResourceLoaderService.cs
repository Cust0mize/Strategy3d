using UnityEngine;
using System.IO;

public class ResourceLoaderService {
    private const string CONFIGS_PATH = "Configs/";
    private const string SPRITE_PATH = "Sprites/";
    private const string MUSIC_PATH = "Music";
    private const string SFX_PATH = "Sfx";

    public TextAsset GetConfigFile(string path) {
        string fullPath = Path.Combine(CONFIGS_PATH, path);
        return Resources.Load<TextAsset>(fullPath);
    }

    public AudioClip GetMusic(string name) {
        string fullPath = Path.Combine(MUSIC_PATH, name);
        return Resources.Load<AudioClip>(fullPath);
    }

    public AudioClip GetSfx(string name) {
        string fullPath = Path.Combine(SFX_PATH, name);
        return Resources.Load<AudioClip>(fullPath);
    }

    public Sprite GetSprite(string image) {
        string fullPath = Path.Combine(SPRITE_PATH, image);
        return Resources.Load<Sprite>(fullPath);
    }
}
