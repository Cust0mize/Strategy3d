using Scripts.Game.Unit;
using UnityEngine;

namespace Scripts.Game.Components {
    public class UnitSelectionComponent : BaseSelectionComponent {
        [SerializeField] private BaseUnit _baseUnit;

        public override void Select() {
            print("Unit selection");
        }

        public override void Unselect() {
        }
    }
}