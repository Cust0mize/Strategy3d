using Assets.Game.Scripts.Signals;
using System.Collections.Generic;
using Scripts.Game.Components;
using UnityEngine;
using Zenject;

namespace Scripts.Game.Cell {
    public class CellManager : MonoBehaviour {
        public Dictionary<int, List<RoadElement>> RoadsDictionary { get; private set; } = new();
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

        private void Start() {
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

        //private Graph CreateTransitionGraph() {
        //    var graph = new Graph();
        //    var firstNode = new Node(TransitionDirection.Left);
        //    var secondNode = new Node(TransitionDirection.UpLeft);
        //    var thirdNode = new Node(TransitionDirection.DownLeft);
        //    var fourthNode = new Node(TransitionDirection.UpRight);
        //    var fifthNode = new Node(TransitionDirection.DownRight);
        //    var sixthNode = new Node(TransitionDirection.Right);
        //    firstNode.AddNewNode(secondNode);
        //    firstNode.AddNewNode(thirdNode);
        //    thirdNode.AddNewNode(fifthNode);
        //    secondNode.AddNewNode(fourthNode);
        //    fourthNode.AddNewNode(sixthNode);
        //    fifthNode.AddNewNode(sixthNode);
        //    graph.Graphs.AddRange(new List<Node> { firstNode, secondNode, thirdNode, fourthNode, fifthNode, sixthNode });
        //    return graph;
        //}

        //public int GetDistanceToNode(TransitionDirection start, TransitionDirection end) {
        //    var graph = CreateTransitionGraph();
        //    var result = graph.GetDistanseToNode(graph.GetNode(start), graph.GetNode(end));
        //    return result;
        //}
    }
}