using UnityEngine;

public class AudioMixerManager : MonoBehaviour {
    [field: SerializeField, Range(0f, 1f)] public float SoundName { get; private set; } = 0.5f;
}