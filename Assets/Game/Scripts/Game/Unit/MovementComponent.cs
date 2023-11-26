using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MovementComponent : MonoBehaviour {
    [SerializeField] private Transform _movementTransform;
    [field: SerializeField] public int StepLength { get; private set; } = 100;

    public async void Move(List<BaseCell> baseCells) {
        for (int i = 0; i < baseCells.Count; i++) {
            float startTime = 0;
            while (transform.position != baseCells[i].transform.position) {
                transform.position = Vector3.Lerp(transform.position, baseCells[i].transform.position, startTime);
                startTime += Time.deltaTime;
                await UniTask.DelayFrame(1, cancellationToken: this.GetCancellationTokenOnDestroy());
            }
        }
    }
}