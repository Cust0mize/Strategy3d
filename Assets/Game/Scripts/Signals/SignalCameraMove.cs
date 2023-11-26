using UnityEngine;

namespace Assets.Game.Scripts.Signals {
    public class SignalCameraMove {
        public Vector3 Direction { get; private set; }

        public SignalCameraMove(Vector3 direction) {
            Direction = direction;
        }
    }
}