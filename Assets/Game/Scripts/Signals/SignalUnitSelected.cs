namespace Assets.Game.Scripts.Signals {
    public class SignalUnitSelected {
        public readonly BaseCell SelectedCell;

        public SignalUnitSelected(BaseCell baseCell) {
            SelectedCell = baseCell;
        }
    }

    public class DisableCells {

    }
}