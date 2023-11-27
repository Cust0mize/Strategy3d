using Scripts.Game.Components;
using UnityEngine;

namespace Scripts.Game.Unit {
    public class BaseUnit : MonoBehaviour {
        [field: SerializeField] public UnitSelectionComponent UnitSelectionComponent { get; private set; }
        [field: SerializeField] public MovementComponent MovementComponent { get; private set; }
    }
}
