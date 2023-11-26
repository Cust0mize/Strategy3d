using UnityEngine;

public class UnitSelectionComponent : BaseSelectionComponent {
    [SerializeField] private BaseUnit _baseUnit;

    public override void Select() {
        print("Unit selection");
    }

    public override void Unselect() {
    }
}
