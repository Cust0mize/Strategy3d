using UnityEngine;

namespace ScreenBlur {
    [ExecuteInEditMode]
    public class SuperBlurBase : MonoBehaviour {
        protected static class Uniforms {
            public static readonly int _Radius = Shader.PropertyToID("_Radius");
            public static readonly int _BackgroundTexture = Shader.PropertyToID("_SuperBlurTexture");
        }

        public Color AdditiveColor = Color.clear;
        public Color Tint = Color.white;

        public RenderMode renderMode = RenderMode.Screen;
        public BlurKernelSize kernelSize = BlurKernelSize.Small;

        [Range(0f, 1f)] public float interpolation = 1f;
        [Range(0, 4)] public int downsample = 1;
        [Range(1, 8)] public int iterations = 1;

        public bool gammaCorrection = true;

        public Material blurMaterial;
        public Material UIMaterial;

        Material _blurMaterialInstance;
        Material _uiMaterialInstance;

        protected void Awake() {
            //_blurMaterialInstance = new Material(blurMaterial);
            //_uiMaterialInstance = new Material(UIMaterial);
        }

        protected void OnDestroy() {
            if (Application.isPlaying) {
                if (_blurMaterialInstance) {
                    Destroy(_blurMaterialInstance);
                }
                if (_uiMaterialInstance) {
                    Destroy(_uiMaterialInstance);
                }
            }
            else {
                if (_blurMaterialInstance) {
                    DestroyImmediate(_blurMaterialInstance);
                }
                if (_uiMaterialInstance) {
                    DestroyImmediate(_uiMaterialInstance);
                }
            }
        }

        protected void Blur(RenderTexture source, RenderTexture destination) {
            if (gammaCorrection) {
                Shader.EnableKeyword("GAMMA_CORRECTION");
            }
            else {
                Shader.DisableKeyword("GAMMA_CORRECTION");
            }

            var kernel = 0;
            switch (kernelSize) {
                case BlurKernelSize.Small:
                    kernel = 0;
                    break;
                case BlurKernelSize.Medium:
                    kernel = 2;
                    break;
                case BlurKernelSize.Big:
                    kernel = 4;
                    break;
            }

            var rt2 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);

            for (var i = 0; i < iterations; i++) {
                // helps to achieve a larger blur
                var radius = i * interpolation + interpolation;
                _blurMaterialInstance.SetFloat(Uniforms._Radius, radius);

                Graphics.Blit(source, rt2, _blurMaterialInstance, 1 + kernel);
                source.DiscardContents();

                Graphics.Blit(rt2, source, _blurMaterialInstance, 2 + kernel);
                rt2.DiscardContents();
            }

            Graphics.Blit(source, destination, _blurMaterialInstance, 7);
            source.DiscardContents();

            RenderTexture.ReleaseTemporary(rt2);
        }

        protected virtual void Update() {
            if (_blurMaterialInstance) {
                _blurMaterialInstance.SetColor("_ColorMultiply", Tint);
                _blurMaterialInstance.SetColor("_ColorAdditive", AdditiveColor);
            }
            if (_uiMaterialInstance) {
                _uiMaterialInstance.SetColor("_ColorMultiply", Tint);
                _uiMaterialInstance.SetColor("_ColorAdditive", AdditiveColor);
            }
        }

    }

    public enum BlurKernelSize {
        Small,
        Medium,
        Big
    }

    public enum RenderMode {
        Screen,
        UI,
        OnlyUI
    }

}