using System.Collections.Generic;

namespace Assets.Game.Scripts.Signals {
    public class SignalGridCreated {
        public readonly List<BaseCell> BaseCellsList;
        public readonly BaseCell[,] BaseCellsArray;

        public SignalGridCreated(List<BaseCell> baseCellsList, BaseCell[,] baseCellsArray) {
            BaseCellsArray = baseCellsArray;
            BaseCellsList = baseCellsList;
        }
    }
}