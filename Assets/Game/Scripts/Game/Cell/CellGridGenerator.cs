using Random = UnityEngine.Random;
using Assets.Game.Scripts.Signals;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Scripts.Game.Cell {
    public class CellGridGenerator : MonoBehaviour {
        private List<BaseCell> _allCellsList = new();
        [SerializeField] private List<BaseCell> _prefabs;
        [SerializeField] private float _zOffset;
        [SerializeField] private float _xOffset;
        [SerializeField] private float _optionallyOffset = -1f;
        [SerializeField] private float _dopZOffset = 0.5f;
        [SerializeField] private int _zSize = 50;
        [SerializeField] private int _xSize = 50;
        private BaseCell[,] _allCellsArray;
        private int _currentGropCount;
        private int groupCount;
        private DiContainer _diContainer;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus, DiContainer diContainer) {
            _diContainer = diContainer;
            _signalBus = signalBus;
        }

        void Start() {
            Generate();
        }

        private void OnValidate() {
            foreach (var cell in _allCellsList) {
                cell.UpdatePosition(_zOffset, _xOffset, _optionallyOffset, _dopZOffset);
            }
        }

        private void Generate() {
            _allCellsList.Clear();
            _allCellsArray = new BaseCell[_zSize, _xSize];
            _currentGropCount = groupCount;

            for (int z = 0; z < _zSize; z++) {
                for (int x = 0; x < _xSize; x++) {
                    BaseCell cell;
                    if (z == 0 && x < 2) {
                        cell = _diContainer.InstantiatePrefabForComponent<BaseCell>(_prefabs[0]);
                    }
                    else {
                        cell = _diContainer.InstantiatePrefabForComponent<BaseCell>(_prefabs[Random.Range(1, _prefabs.Count)]);
                    }

                    if (z % 2 == 0 && z != _currentGropCount) {
                        groupCount += 2;
                        _currentGropCount = groupCount;
                    }
                    cell.Init(transform.position, z, x, groupCount);
                    _allCellsList.Add(cell);
                    _allCellsArray[x, z] = cell;
                    cell.UpdatePosition(_zOffset, _xOffset, _optionallyOffset, _dopZOffset);
                    cell.AddNeighbor(AddNeighbor(cell));
                }
            }
            _signalBus.Fire(new SignalGridCreated(_allCellsList, _allCellsArray));
        }

        private List<BaseCell> AddNeighbor(BaseCell cellBase) {
            var newCells = new List<BaseCell>();
            var xOffset = cellBase.ZCount % 2 == 0 ? new[] { -1, 1, 0, 1, 0, -1 } : new[] { -1, 1, 0, -1, 0, 1 };
            var zOffset = new[] { 0, 0, -1, -1, 1, 1 };

            for (int i = 0; i < xOffset.Length; i++) {
                var cell = _allCellsList.FirstOrDefault(cell =>
                    cell.ZCount == cellBase.ZCount + zOffset[i] && cell.XCount == cellBase.XCount + xOffset[i]);

                if (cell != null) {
                    newCells.Add(cell);
                    cell.AddNeighbor(cellBase);
                }
            }

            return newCells;
        }
    }
}