using Assets.Game.Scripts.Signals;
using System.Collections.Generic;
using Scripts.Game.Cell;
using Zenject;

public class PathFinding  {
    private List<BaseCell> _allCellsList = new();
    private List<BaseCell> _closeList = new();
    private List<BaseCell> _openList = new();

    [Inject]
    private void Construct(SignalBus signalBus) {
        signalBus.Subscribe<SignalGridCreated>(SetGrid);
    }

    private void SetGrid(SignalGridCreated signalGridCreated) {
        _allCellsList = signalGridCreated.BaseCellsList;
    }

    public List<BaseCell> FingPath(BaseCell startCell, BaseCell endCell) {
        _openList = new List<BaseCell> { startCell };
        _closeList = new List<BaseCell>();

        foreach (var cell in _allCellsList) {
            cell.SetStartValue();
        }

        startCell.SetStartCellValue(endCell);

        while (_openList.Count > 0) {
            var currentCell = GetLowestFCostCell(_openList);

            if (currentCell == endCell) {
                var result = CalculatePath(endCell);
                foreach (var item in _allCellsList) {
                    item.SetDefaultValue();
                }
                return result;
            }
            _openList.Remove(currentCell);
            _closeList.Add(currentCell);

            foreach (var neighbors in currentCell.CellNeighbors) {
                if (_closeList.Contains(neighbors)) {
                    continue;
                }
                if (!neighbors.IsMovebleCell || neighbors.BaseUnit != null) {
                    continue;
                }

                int tentativeGCost = currentCell.GetTentativeGCost(neighbors);

                if (tentativeGCost < neighbors.GCost) {
                    neighbors.SetNeighborValue(currentCell, tentativeGCost, endCell);

                    if (!_openList.Contains(neighbors)) {
                        _openList.Add(neighbors);
                    }
                }
            }
        }
        return null;
    }

    private List<BaseCell> CalculatePath(BaseCell endCell) {
        List<BaseCell> path = new List<BaseCell>();
        path.Add(endCell);
        BaseCell currentCell = endCell;
        while (currentCell.CameFromTile != null) {
            path.Add(currentCell.CameFromTile);
            currentCell = currentCell.CameFromTile;
        }
        path.Reverse();
        return path;
    }

    private BaseCell GetLowestFCostCell(List<BaseCell> cells) {
        var lowestCell = cells[0];
        for (int i = 0; i < cells.Count; i++) {
            if (cells[i].FCost < lowestCell.FCost) {
                lowestCell = cells[i];
            }
        }
        return lowestCell;
    }
}
