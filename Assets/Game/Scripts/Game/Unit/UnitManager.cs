using Assets.Game.Scripts.Signals;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class UnitManager : MonoBehaviour {
    private PathFinding _pathFinding;
    private List<BaseUnit> _allUnits = new List<BaseUnit>();
    private BaseUnit _selectedUnit;
    private BaseCell startCell;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(
    PathFinding pathFinding,
    SignalBus signalBus
    ) {
        _pathFinding = pathFinding;
        _signalBus = signalBus;
        signalBus.Subscribe<SignalRightRaycastHit>(TryMoveUnit);
        signalBus.Subscribe<SignalUnitSelected>(ChangeCellSelected);
    }

    private void ChangeCellSelected(SignalUnitSelected singalCellSelected) {
        startCell = singalCellSelected.SelectedCell;
        _selectedUnit = singalCellSelected.SelectedCell.BaseUnit;
        _selectedUnit.UnitSelectionComponent.Select();
    }

    private void TryMoveUnit(SignalRightRaycastHit signalRightRaycastHit) {
        if (signalRightRaycastHit.Hit.collider.TryGetComponent(out BaseCell endCell)) {
            if (!endCell.IsMovebleCell || !endCell.IsEmptyCell || _selectedUnit == null) {
                return;
            }

            if (_selectedUnit.TryGetComponent(out MovementComponent movementComponent)) {
                _selectedUnit.UnitSelectionComponent.Unselect();
                startCell.ClearCell();
                movementComponent.Move(_pathFinding.FingPath(startCell, endCell));
                endCell.TryFillCell(_selectedUnit);
                _signalBus.Fire(new DisableCells());
                _selectedUnit = null;
                startCell = null;
            }
        }
    }

    private void Start() {
        _allUnits = FindObjectsOfType<BaseUnit>().ToList();
    }
}