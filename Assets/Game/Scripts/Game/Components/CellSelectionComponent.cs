using Assets.Game.Scripts.Signals;
using Scripts.Game.Cell;
using UnityEngine;

namespace Scripts.Game.Components {
    public class CellSelectionComponent : BaseSelectionComponent {
        [field: SerializeField] public BaseCell CellBase { get; private set; }

        public override void Select() {
            SignalBus.Fire(new SignalDisableCells());
            print("selection " + gameObject.name);
        }

        public override void Unselect() {
        }
    }
}
