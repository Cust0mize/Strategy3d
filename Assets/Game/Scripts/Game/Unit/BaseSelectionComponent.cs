using Assets.Game.Scripts.Signals;
using UnityEngine;
using Zenject;

public abstract class BaseSelectionComponent : MonoBehaviour {
    protected SignalBus SignalBus { get; private set; }

    [Inject]
    private void Construct(SignalBus signalBus) {
        SignalBus = signalBus;
    }

    public abstract void Select();
    public abstract void Unselect();
}

