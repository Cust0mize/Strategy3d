using UnityEngine;
using Zenject;

[RequireComponent(typeof(UIService), typeof(PathFinding))]
public class GameSceneInstaller : MonoInstaller {
    //[SerializeField] private ScreenBlurController _screenBlurController;

    public override void InstallBindings() {
        Container.Bind<UIService>().FromComponentInHierarchy().AsTransient().NonLazy();     //В себе ищет панели на сцене, поэтому нужно заново инстансить в каждой сцене, либо вынести логику поиска в метод инита и реинитить в стартере.
        //Container.Bind<ScreenBlurController>().FromInstance(_screenBlurController).AsSingle().NonLazy();
        Container.Bind<PathFinding>().FromComponentInHierarchy().AsTransient().NonLazy();
    }
}
