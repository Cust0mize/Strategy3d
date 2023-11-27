using UnityEngine;
using Zenject;

namespace Scripts.Game.Components {
    public abstract class BaseSelectionComponent : MonoBehaviour {
        protected SignalBus SignalBus { get; private set; }

        [Inject]
        private void Construct(SignalBus signalBus) {
            SignalBus = signalBus;
        }

        public abstract void Select();
        public abstract void Unselect();
    }
}