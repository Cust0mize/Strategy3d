using System.Collections.Generic;
using Scripts.Game.Cell;

public class ReachableUtils {
    public List<BaseCell> FindReachableCells(BaseCell startCell, int stepLength) {
        List<BaseCell> openList = new();
        List<BaseCell> closeList = new();
        List<BaseCell> reachableCells = new() { startCell };
        int startStepLength = stepLength;

        while (stepLength > 0) {
            List<BaseCell> newReachableCells = new();

            for (int cellIndex = 0; cellIndex < reachableCells.Count; cellIndex++) {
                BaseCell cell = reachableCells[cellIndex];
                for (int neighborsIndex = 0; neighborsIndex < cell.CellNeighbors.Count; neighborsIndex++) {
                    BaseCell neighbor = cell.CellNeighbors[neighborsIndex];
                    if (!reachableCells.Contains(neighbor) && !newReachableCells.Contains(neighbor) && !openList.Contains(neighbor) && !closeList.Contains(neighbor)) {
                        if (neighbor.GetTentativeGCost(startCell) <= startStepLength && neighbor.IsEmptyCell) {
                            newReachableCells.Add(neighbor);
                        }
                    }
                }
            }

            closeList.AddRange(reachableCells);
            openList.AddRange(newReachableCells);
            reachableCells = newReachableCells;
            stepLength--;
        }

        return openList;
    }
}