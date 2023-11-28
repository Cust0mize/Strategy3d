using Assets.Game.Scripts.Signals;
using UnityEngine;
using Zenject;

public class InputSystem : MonoBehaviour {
    private SignalBus _signalBus;
    private Camera _camera;
    private Ray _ray;

    private void Start() {
        _ray = new Ray(transform.position, transform.forward);
        _camera = Camera.main;
    }

    [Inject]
    private void Construct(SignalBus signalBus) {
        _signalBus = signalBus;
    }

    private void Update() {
        MouseInput();
        KeyboardInput();
    }

    private void KeyboardInput() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0) {
            _signalBus.Fire(new SignalCameraMove(new Vector3(horizontal, 0, 0)));
        }

        if (vertical != 0) {
            _signalBus.Fire(new SignalCameraMove(new Vector3(0, 0, vertical)));
        }
    }

    private void MouseInput() {
        if (Input.GetMouseButtonDown(0)) {
            if (RayHit(out RaycastHit raycastHit)) {
                _signalBus.Fire(new SignalLeftRaycastHit(raycastHit));
            }
        }

        if (Input.GetMouseButtonUp(1)) {
            if (RayHit(out RaycastHit raycastHit)) {
                _signalBus.Fire(new SignalRightRaycastHit(raycastHit));
            }
        }
    }

    private float CalculateDirection(float position, float screenSize, float boardSize) {
        float direction = 0;

        if (position < boardSize) {
            direction = Mathf.Clamp(position / boardSize - 1f, -1f, 0f);
        }
        else if (position > screenSize - boardSize) {
            direction = Mathf.Clamp01(Mathf.Abs((position - screenSize) / boardSize + 1f));
        }

        return direction;
    }

    private bool RayHit(out RaycastHit raycastHit) {
        _ray.origin = _camera.transform.position;
        _ray.direction = _camera.ScreenPointToRay(Input.mousePosition).direction;
        return Physics.Raycast(_ray, out raycastHit);
    }
}
