using Assets.Game.Scripts.Signals;
using Scripts.Game.Components;
using Scripts.Game.Cell;
using UnityEngine;
using Zenject;

namespace Scripts.Game.Unit {
    public class UnitManager : MonoBehaviour {
        private PathFinding _pathFinding;
        private BaseUnit _selectedUnit;
        private SignalBus _signalBus;
        private BaseCell startCell;

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
                    _signalBus.Fire(new SignalDisableCells());
                    _selectedUnit = null;
                    startCell = null;
                }
            }
        }
    }
}