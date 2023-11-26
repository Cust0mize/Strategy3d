using Assets.Game.Scripts.Signals;
using UnityEngine;

public class CellSelectionComponent : BaseSelectionComponent {
    [field: SerializeField] public BaseCell CellBase { get; private set; }

    public override void Select() {
        SignalBus.Fire(new DisableCells());
        print("selection " + gameObject.name);
    }

    public override void Unselect() {
    }
}
