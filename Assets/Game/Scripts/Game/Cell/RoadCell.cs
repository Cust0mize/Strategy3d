using Assets.Game.Scripts.Signals;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using System.Linq;

namespace Scripts.Game.Cell {
    public class RoadCell : BaseCell {
        [SerializeField] private List<RoadElement> _roadElements;
        private Dictionary<int, List<RoadElement>> _roadsDictionary = new();
        private Dictionary<TransitionDirection, bool> _transitions = new();

        public override void Init(Vector3 position, int z, int j, int groupCount) {
            base.Init(position, z, j, groupCount);

            System.Collections.IList list = Enum.GetValues(typeof(TransitionDirection));
            for (int i = 0; i < list.Count; i++) {
                object item = list[i];
                _transitions.Add((TransitionDirection)item, false);
            }

            for (int i = 0; i < _roadElements.Count; i++) {
                _roadElements[i].ConvertTransitionDirectionToBinaryValue();
                _roadElements[i].gameObject.SetActive(false);
                if (_roadsDictionary.ContainsKey(_roadElements[i].RoadContactNubmer)) {
                    _roadsDictionary[_roadElements[i].RoadContactNubmer].Add(_roadElements[i]);
                }
                else {
                    _roadsDictionary.Add(_roadElements[i].RoadContactNubmer, new List<RoadElement> { _roadElements[i] });
                }
            }
        }

        [ContextMenu("GetInfo")]
        public void GetInfo() {
            foreach (var item in _transitions) {
                print($"{item.Key} / {item.Value}");
            }
        }

        [Inject]
        private void Construct(SignalBus signalBus) {
            signalBus.Subscribe<SignalGridCreated>(SearchSimilarNeighbors);
        }

        private void SearchSimilarNeighbors(SignalGridCreated signalGridCreated) {
            int numberNeighborsSameType = 0;

            for (int i = 0; i < CellNeighbors.Count; i++) {
                if (CellNeighbors[i].GetType() == GetType()) {
                    numberNeighborsSameType++;
                    SearchConnecteds(i);
                }
            }
            List<TransitionDirection> transitions = new();
            foreach (var item in _transitions) {
                if (item.Value) {
                    transitions.Add(item.Key);
                }
            }
            RoadElementUtils roadElementUtils = new RoadElementUtils();
            string binaruElement = roadElementUtils.GetBinaryValue(transitions, out int count);
            string searchElement = roadElementUtils.ReplaceChar(binaruElement, _roadsDictionary[count], out int rotateCount);
            RoadElement element = _roadsDictionary[count].FirstOrDefault(x => x.BinaryValue == searchElement);
            element.gameObject.transform.Rotate(0, rotateCount * -60, 0);


            element.gameObject.SetActive(true);
        }

        [ContextMenu("GetBinaryInfo")]
        public void GetBinaryInfo() {
            List<TransitionDirection> transitions = new();
            foreach (var item in _transitions) {
                if (item.Value) {
                    transitions.Add(item.Key);
                }
            }

            RoadElementUtils roadElementUtils = new RoadElementUtils();
            string binaruElement = roadElementUtils.GetBinaryValue(transitions, out int count);
            roadElementUtils.ReplaceChar(binaruElement, _roadsDictionary[count], out int rotateCount);
        }

        private void SearchConnecteds(int i) {
            if (CellNeighbors[i].ZPosition == ZPosition) {
                if (CellNeighbors[i].XPosition - XPosition == -1) {
                    _transitions[TransitionDirection.Left] = true;
                }
                else {
                    _transitions[TransitionDirection.Right] = true;
                }
            }
            else if (CellNeighbors[i].ZPosition > ZPosition) {
                if (IsEvenRow) {
                    if (CellNeighbors[i].XPosition - XPosition == 0) {
                        _transitions[TransitionDirection.UpLeft] = true;
                    }
                    else {
                        _transitions[TransitionDirection.UpRight] = true;
                    }
                }
                else {
                    if (CellNeighbors[i].XPosition - XPosition == -1) {
                        _transitions[TransitionDirection.UpLeft] = true;
                    }
                    else {
                        _transitions[TransitionDirection.UpRight] = true;
                    }
                }
            }
            else {
                if (IsEvenRow) {
                    if (CellNeighbors[i].XPosition - XPosition == 0) {
                        _transitions[TransitionDirection.DownLeft] = true;
                    }
                    else {
                        _transitions[TransitionDirection.DownRight] = true;
                    }
                }
                else {
                    if (CellNeighbors[i].XPosition - XPosition == -1) {
                        _transitions[TransitionDirection.DownLeft] = true;
                    }
                    else {
                        _transitions[TransitionDirection.DownRight] = true;
                    }
                }
            }
        }
    }
}

public enum TransitionDirection {
    None = -99,
    Left,
    UpLeft,
    UpRight,
    Right,
    DownRight,
    DownLeft,
}