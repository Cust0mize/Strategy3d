using Assets.Game.Scripts.Signals;
using System.Collections.Generic;
using Scripts.Game.Components;
using UnityEngine;
using Zenject;

namespace Scripts.Game.Cell {
    public class CellManager : MonoBehaviour {
        private List<BaseCell> _shadeCells = new List<BaseCell>();
        private CellSelectionComponent _oldSelectionCell;
        private ReachableUtils _reachableUtils;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(
        ReachableUtils reachableUtils,
        SignalBus signalBus
        ) {
            _reachableUtils = reachableUtils;
            _signalBus = signalBus;
            _signalBus.Subscribe<SignalLeftRaycastHit>(TrySelect);
            _signalBus.Subscribe<SignalDisableCells>(DisableCell);
        }

        private void TrySelect(SignalLeftRaycastHit signalRaycastHit) {
            if (signalRaycastHit.Hit.collider.TryGetComponent(out CellSelectionComponent newSelectionCell)) {
                if (_oldSelectionCell == newSelectionCell) {
                    return;
                }
                _oldSelectionCell?.Unselect();
                _oldSelectionCell = newSelectionCell;
                _oldSelectionCell.Select();
                if (_oldSelectionCell.CellBase.BaseUnit) {
                    EnableCell(_oldSelectionCell.CellBase);
                    _signalBus.Fire(new SignalUnitSelected(_oldSelectionCell.CellBase));
                }
            }
        }

        private void EnableCell(BaseCell baseCell) {
            _shadeCells = _reachableUtils.FindReachableCells(baseCell, baseCell.BaseUnit.MovementComponent.StepLength);

            foreach (var item in _shadeCells) {
                item.AllowMovement();
                item.OnShade();
            }
        }

        private void DisableCell() {
            foreach (var item in _shadeCells) {
                item.BannedMovement();
                item.OffShade();
            }
            _shadeCells.Clear();
        }
    }
}