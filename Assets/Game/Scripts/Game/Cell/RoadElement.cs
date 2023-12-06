using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Zenject;
using System;

namespace Scripts.Game.Cell {
    public class RoadElement : MonoBehaviour {
        [SerializeField] private Transform _roadRoot;
        [field: SerializeField] public int RoadContactNubmer { get; private set; }
        [SerializeField] private List<TransitionDirection> _transitionDirections;
        public string BinaryValue { get; private set; }
        public int ConnectCount { get; private set; }

        public void ConvertTransitionDirectionToBinaryValue() {
            RoadElementUtils roadElementUtils = new RoadElementUtils();
            BinaryValue = roadElementUtils.GetBinaryValue(_transitionDirections, out int count);
            ConnectCount = count;
        }
    }

    public class RoadElementUtils {
        private Dictionary<TransitionDirection, bool> _binaryValues = new();
        private StringBuilder BinaryValue = new();
        private CellManager _cellManager;

        [Inject]
        private void Construct(CellManager cellManager) {
            _cellManager = cellManager;
        }

        public string GetBinaryValue(List<TransitionDirection> transitionDirections, out int connectCount) {
            BinaryValue.Clear();
            connectCount = 0;

            foreach (var item in Enum.GetValues(typeof(TransitionDirection))) {
                if ((TransitionDirection)item == TransitionDirection.None) {
                    continue;
                }
                _binaryValues.Add((TransitionDirection)item, false);
            }

            foreach (var item in _binaryValues) {
                if (transitionDirections.Contains(item.Key)) {
                    BinaryValue.Append("1");
                    connectCount++;
                }
                else {
                    BinaryValue.Append("0");
                }
            }
            return BinaryValue.ToString();
        }

        public string ReplaceChar(string currentBinaryValue, List<RoadElement> targetsList, out int rotateCount) {
            string startValue = currentBinaryValue;
            BinaryValue.Clear();
            rotateCount = 0;
            bool isBreak = false;

            for (int i = 0; i < targetsList.Count; i++) {
                rotateCount = 0;
                for (int j = 0; j < currentBinaryValue.Length; j++) {
                    if (targetsList[i].BinaryValue == currentBinaryValue) {
                        Debug.Log($"{startValue} == {currentBinaryValue}, Rotate {rotateCount}");
                        isBreak = true;
                        break;
                    }
                    currentBinaryValue = currentBinaryValue.Substring(currentBinaryValue.Length - 1, 1) + currentBinaryValue.Substring(0, currentBinaryValue.Length - 1);
                    rotateCount++;
                }
                if (isBreak) {
                    break;
                }
            }

            return currentBinaryValue;
        }
    }
}