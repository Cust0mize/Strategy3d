using System.Collections.Generic;
using System.Linq;

namespace Scripts.GraphTest {
    public class Graph {
        public readonly List<Node> Graphs = new();

        public Node GetNode(TransitionDirection transitionDirection) {
            return Graphs.FirstOrDefault(x => x._currentTransitionDirection == transitionDirection);
        }

        public int GetDistanseToNode(Node startNode, Node endNode) {
            var queue = new Queue<Node>();
            var visited = new HashSet<Node>();
            var distances = new Dictionary<Node, int>();

            queue.Enqueue(startNode);
            visited.Add(startNode);
            distances[startNode] = 0;

            while (queue.Count > 0) {
                var currentNode = queue.Dequeue();

                if (currentNode == endNode) {
                    int result = distances[currentNode];
                    return result;
                }

                foreach (var connectedNode in currentNode.ConnectedNodes) {
                    if (!visited.Contains(connectedNode)) {
                        queue.Enqueue(connectedNode);
                        visited.Add(connectedNode);
                        distances[connectedNode] = distances[currentNode] + 1;
                    }
                }
            }

            return -1;
        }
    }
}