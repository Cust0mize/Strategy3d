using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Cell {
    public class RoadElement : MonoBehaviour {
        [field: SerializeField] public int RoadContactNubmer { get; private set; }
        [SerializeField] private List<TransitionDirection> _transitionDirections;
    }
}