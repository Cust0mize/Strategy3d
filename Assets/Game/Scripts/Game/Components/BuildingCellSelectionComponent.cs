using Assets.Game.Scripts.Signals;

namespace Scripts.Game.Components {
    public class BuildingCellSelectionComponent : BaseSelectionComponent {
        public override void Select() {
            SignalBus.Fire(new SignalDisableCells());
        }

        public override void Unselect() {
        }
    }
}