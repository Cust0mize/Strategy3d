using UnityEngine;

namespace Assets.Game.Scripts.Signals {
    public class SignalRightRaycastHit {
        public readonly RaycastHit Hit;

        public SignalRightRaycastHit(RaycastHit hit) {
            Hit = hit;
        }
    }
}