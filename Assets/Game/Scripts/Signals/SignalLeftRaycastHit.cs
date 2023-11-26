using UnityEngine;

namespace Assets.Game.Scripts.Signals {
    public class SignalLeftRaycastHit {
        public readonly RaycastHit Hit;

        public SignalLeftRaycastHit(RaycastHit hit) {
            Hit = hit;
        }
    }
}