using System.Collections.Generic;
using Scripts.Game.Unit;
using UnityEngine;
using TMPro;

namespace Scripts.Game.Cell {
    public abstract class BaseCell : MonoBehaviour {
        [SerializeField] private TextMeshPro _cellText;
        [field: SerializeField] public bool IsBuildingCell { get; private set; }
        [field: SerializeField] public bool IsEmptyCell { get; private set; }
        [field: SerializeField] public int StepCost { get; private set; }
        [field: SerializeField] public BaseUnit BaseUnit { get; private set; }
        public bool IsMovebleCell { get; private set; }

        private Material _cellMaterial;
        private Color _normalColor;
        private Color _selectColor = Color.red;

        public List<BaseCell> CellNeighbors { get; private set; } = new();
        public int ZCount { get; private set; }
        public int XCount { get; private set; }
        private Vector3 _startPosition;
        private int _groupCount;

        public int GCost { get; private set; } = 0;
        public int HCost { get; private set; } = 0;
        public int FCost { get; private set; } = 0;
        public BaseCell CameFromTile { get; private set; }

        public void Init(Vector3 position, int z, int j, int groupCount) {
            _startPosition = position;
            ZCount = z;
            XCount = j;
            _groupCount = groupCount;
            _cellText.text = $"{ZCount}{XCount}";
            gameObject.name = $"{ZCount}{XCount}";
            _cellMaterial = GetComponentInChildren<MeshRenderer>().material;
            _normalColor = _cellMaterial.color;
        }

        public void UpdatePosition(float _xOffset, float yOffset, float noOffsetX, float dopZOffxet) {
            if (ZCount % 2 == 0) {
                transform.position = new Vector3(_startPosition.x + yOffset * XCount, _startPosition.y, _startPosition.z + _xOffset * ZCount);
            }
            else {
                transform.position = new Vector3(_startPosition.x + yOffset * XCount + noOffsetX, _startPosition.y, _startPosition.z + _xOffset * ZCount + noOffsetX);
            }

            if (ZCount > 1) {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + _groupCount * dopZOffxet);
            }
        }

        public void AddNeighbor(List<BaseCell> cellBases) {
            CellNeighbors.AddRange(cellBases);
        }

        public void AddNeighbor(BaseCell cellBases) {
            CellNeighbors.Add(cellBases);
        }

        public void AllowMovement() {
            IsMovebleCell = true;
        }

        public void BannedMovement() {
            IsMovebleCell = false;
        }

        public void TryFillCell(BaseUnit baseUnit) {
            if (!IsMovebleCell || !IsEmptyCell) {
                print("Невозможно поставить юнита в эту клетку");
                return;
            }
            IsEmptyCell = false;
            BaseUnit = baseUnit;
        }

        public void OnShade() {
            _cellMaterial.color = _selectColor;
        }

        public void OffShade() {
            _cellMaterial.color = _normalColor;
        }

        public void ClearCell() {
            IsEmptyCell = true;
            BaseUnit = null;
        }

        [ContextMenu("HideNeighbor")]
        public void HideNeighbor() {
            foreach (var cell in CellNeighbors) {
                cell.gameObject.SetActive(false);
            }
        }

        [ContextMenu("ShowNeighbor")]
        public void ShowNeighbor() {
            foreach (var cell in CellNeighbors) {
                cell.gameObject.SetActive(true);
            }
        }

        public void CalculateFCost() {
            FCost = HCost + GCost;
        }

        public void SetStartValue() {
            GCost = int.MaxValue;
            CalculateFCost();
            CameFromTile = null;
        }

        public void SetDefaultValue() {
            GCost = 0;
            HCost = 0;
            CameFromTile = null;
            CalculateFCost();
        }

        public void SetStartCellValue(BaseCell endCell) {
            GCost = 0;
            HCost = CalculateDistanceToTarget(endCell);
            CalculateFCost();
        }

        public int GetTentativeGCost(BaseCell targetCell) {
            print(GCost + CalculateDistanceToTarget(targetCell));
            return GCost + CalculateDistanceToTarget(targetCell);
        }

        public void SetNeighborValue(BaseCell currentCell, int tentativeGCost, BaseCell endCell) {
            CameFromTile = currentCell;
            GCost = tentativeGCost;
            HCost = CalculateDistanceToTarget(endCell);
            CalculateFCost();
        }

        private int CalculateDistanceToTarget(BaseCell targetCell) {
            int xDistance = Mathf.Abs(XCount - targetCell.XCount);
            int zDistance = Mathf.Abs(ZCount - targetCell.ZCount);
            int remaining = Mathf.Abs(xDistance - zDistance);
            return StepCost * Mathf.Min(xDistance, zDistance) + StepCost * remaining;
        }
    }
}