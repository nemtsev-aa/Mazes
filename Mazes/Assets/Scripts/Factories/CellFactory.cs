using UnityEngine;
using Zenject;

public class CellFactory {
    private DiContainer _container;
    private Cell _cellPrefab;
    
    private Vector3 _cellSize;

    public CellFactory(DiContainer container, Cell cellPrefab) {
        _cellPrefab = cellPrefab;
        _container = container;
    }

    public void Init(Vector3 cellSize) {
        _cellSize = cellSize;
    }

    public Cell Get(float positionX, float positionY, Transform parent) {
        Vector3 position = new Vector3(positionX * _cellSize.x, positionY * _cellSize.y, positionY * _cellSize.z);
        Quaternion rotation = Quaternion.identity;

        return _container.InstantiatePrefabForComponent<Cell>(_cellPrefab, position, rotation, parent);
    }
}
