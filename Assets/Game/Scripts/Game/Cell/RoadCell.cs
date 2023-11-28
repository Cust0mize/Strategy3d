using Assets.Game.Scripts.Signals;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using ModestTree;
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
                    SearchSochelenenia(i);
                }
            }
            _roadsDictionary[numberNeighborsSameType][0].gameObject.SetActive(true);
        }

        [ContextMenu("Test")]
        public void Test() {
            var graph = new Graph();
            var firstNode = new Node(TransitionDirection.Left);
            var secondNode = new Node(TransitionDirection.UpLeft);
            var thirdNode = new Node(TransitionDirection.DownLeft);
            var fourthNode = new Node(TransitionDirection.UpRight);
            var fifthNode = new Node(TransitionDirection.DownRight);
            var sixthNode = new Node(TransitionDirection.Right);
            firstNode.AddNewNode(secondNode);
            firstNode.AddNewNode(thirdNode);
            thirdNode.AddNewNode(fifthNode);
            secondNode.AddNewNode(fourthNode);
            fourthNode.AddNewNode(sixthNode);
            fifthNode.AddNewNode(sixthNode);
            graph.Graphs.AddRange(new List<Node> { firstNode, secondNode, thirdNode, fourthNode, fifthNode, sixthNode });
            var result = graph.GetDistanseToNode(graph.GetNode(TransitionDirection.Left), graph.GetNode(TransitionDirection.Right));
            print(result);

            List<TransitionDirection> transitions = new();

            foreach (var item in _transitions) {
                if (item.Value) {
                    transitions.Add(item.Key);
                }
            }
        }

        private void SearchSochelenenia(int i) {
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


public class Graph {
    public readonly List<Node> Graphs = new();

    public Node GetNode(TransitionDirection transitionDirection) {
        return Graphs.FirstOrDefault(x => x._currentTransitionDirection == transitionDirection);
    }

    public int GetDistanseToNode(Node startNode, Node endNode) {
        var targetNode = startNode;
        int tryCount = 0;

        while (targetNode != endNode) {
            tryCount++;
            if (targetNode.ConnectedNodes[0] == endNode) {
                break;
            }
            else {
                targetNode = targetNode.ConnectedNodes[0];
            }
            if (tryCount >= 1000) {
                Debug.LogError("HotMnogo");
                break;
            }
        }

        return tryCount;
    }
}

public class Node {
    public TransitionDirection _currentTransitionDirection { get; private set; }
    public readonly List<Node> ConnectedNodes = new();

    public Node(TransitionDirection transitionDirection) {
        _currentTransitionDirection = transitionDirection;
    }

    public void AddNewNode(Node node) {
        ConnectedNodes.Add(node);
        node.ObratAdd(this);
    }

    public void ObratAdd(Node node) {
        ConnectedNodes.Add(node);
    }
}

//public class Graph {
//    public readonly List<Node> Graphs = new();
//}

public enum TransitionDirection {
    None = -99,
    Left = -1,
    Right = 1,
    UpLeft = 2,
    UpRight = 3,
    DownLeft = 4,
    DownRight = 5,
}