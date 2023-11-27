using UnityEngine;
using Zenject;

[RequireComponent(typeof(UIService))]
public class GameSceneInstaller : MonoInstaller {
    //[SerializeField] private ScreenBlurController _screenBlurController;

    public override void InstallBindings() {
        Container.Bind<UIService>().FromComponentInHierarchy().AsCached().NonLazy();     //� ���� ���� ������ �� �����, ������� ����� ������ ���������� � ������ �����, ���� ������� ������ ������ � ����� ����� � ��������� � ��������.
        Container.Bind<ReachableUtils>().AsSingle().NonLazy();
        Container.Bind<PathFinding>().AsSingle().NonLazy();
        //Container.Bind<ScreenBlurController>().FromInstance(_screenBlurController).AsSingle().NonLazy();
    }
}
