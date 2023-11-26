using Assets.Game.Scripts.Signals;
using UnityEngine;
using Zenject;

public class GameCameraController : MonoBehaviour {
    private SignalBus _signalBus;

    [Inject]
    private void Construct(SignalBus signalBus) {
        _signalBus = signalBus;
        _signalBus.Subscribe<SignalCameraMove>(CameraMove);
    }

    private void Start() {

    }

    private void CameraMove(SignalCameraMove signalCameraMove) {
        transform.Translate(signalCameraMove.Direction / 2, Space.World);
    }
}