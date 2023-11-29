using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Scripts.Game.Cell {
    public class RoadElement : MonoBehaviour {
        [field: SerializeField] public int RoadContactNubmer { get; private set; }
        [SerializeField] private List<TransitionDirection> _transitionDirections;
        private Dictionary<TransitionDirection, bool> _binaryValues = new();
        public int Distance { get; private set; }
        //private CellManager _cellManager;
        public string BinaryValue { get; private set; }
        public int ConnectCount { get; private set; }

        private void Start() {
            ConvertTransitionDirectionToBinaryValue();
        }

        private void ConvertTransitionDirectionToBinaryValue() {
            foreach (var item in Enum.GetValues(typeof(TransitionDirection))) {
                if ((TransitionDirection)item == TransitionDirection.None) {
                    continue;
                }
                _binaryValues.Add((TransitionDirection)item, false);
            }

            foreach (var item in _binaryValues) {
                if (_transitionDirections.Contains(item.Key)) {
                    BinaryValue += "1";
                    ConnectCount++;
                }
                else {
                    BinaryValue += "0";
                }
            }
        }

        //[Inject]
        //private void Construct(CellManager cellManager) {
        //    _cellManager = cellManager;
        //}

        //public void SetDistace() {
        //    Distance = 0;
        //    for (int i = 0; i < _transitionDirections.Count; i++) {
        //        if (i == 0) {
        //            continue;
        //        }
        //        Distance += _cellManager.GetDistanceToNode(_transitionDirections[0], _transitionDirections[i]);
        //    }
        //    Distance--;
        //}
    }
}