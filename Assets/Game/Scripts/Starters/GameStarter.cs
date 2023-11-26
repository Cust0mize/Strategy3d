using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Starters {
    public class GameStarter : MonoBehaviour {
        private UIService _uIService;

        [Inject]
        private void Construct(
        UIService uIService
        ) {
            _uIService = uIService;
        }

        private void Start() {
            _uIService.Init();
        }
    }
}