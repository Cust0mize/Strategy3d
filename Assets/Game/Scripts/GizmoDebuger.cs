using UnityEngine;

public class GizmoDebuger : MonoBehaviour {
    private void OnDrawGizmos() {
        Gizmos.DrawCube(Camera.main.WorldToScreenPoint(new Vector3(800, 800, 100)), Vector3.one);
    }
}
