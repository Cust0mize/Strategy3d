using System.Collections.Generic;

namespace Scripts.GraphTest {
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
}