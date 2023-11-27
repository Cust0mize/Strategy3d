using Assets.Game.Scripts.Signals;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CheatManager), typeof(AudioMixerManager), typeof(GlobalAsyncProcessor))]
[RequireComponent(typeof(InputSystem))]
public class ProjectInstaller : MonoInstaller {
    public override void InstallBindings() {
        Container.Bind<AudioMixerManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<GlobalAsyncProcessor>().FromNewComponentOnNewGameObject().AsSingle();
        Container.Bind<CheatManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<InputSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<ParticleAnimationService>().AsSingle().NonLazy();
        Container.Bind<WindowShowHideAnimation>().AsSingle().NonLazy();
        Container.Bind<ResourceLoaderService>().AsSingle().NonLazy();
        Container.Bind<LocalizationService>().AsSingle().NonLazy();
        Container.Bind<SceneLoaderService>().AsSingle().NonLazy();
        Container.Bind<ConfigService>().AsSingle().NonLazy();
        Container.Bind<SaveSystem>().AsSingle().NonLazy();
        Container.Bind<GameData>().AsSingle().NonLazy();

        SignalRegistry();
    }

    public void SignalRegistry() {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<SignalRightRaycastHit>().OptionalSubscriber();
        Container.DeclareSignal<SignalLeftRaycastHit>().OptionalSubscriber();
        Container.DeclareSignal<SignalUnitSelected>().OptionalSubscriber();
        Container.DeclareSignal<SignalGridCreated>().OptionalSubscriber();
        Container.DeclareSignal<SignalCameraMove>().OptionalSubscriber();
        Container.DeclareSignal<SignalDisableCells>().OptionalSubscriber();
    }
}
