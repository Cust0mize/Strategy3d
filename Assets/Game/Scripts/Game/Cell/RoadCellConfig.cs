using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Cell {
    [CreateAssetMenu(fileName = "RoadCellConfig", menuName = "ScriptableObjects/RoadCellConfig", order = 1)]
    public class RoadCellConfig : ScriptableObject {
        public List<RoadElement> _prefabs;
    }
}