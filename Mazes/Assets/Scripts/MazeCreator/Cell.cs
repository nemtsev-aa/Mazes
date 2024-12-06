using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerClickHandler {
    public event Action<Cell> Selected;

    public GameObject WallLeft;
    public GameObject WallBottom;

    public TextMeshProUGUI DistanceLabel;
    public TextMeshProUGUI PositionLabel;

    public MazeCell MazeCell;

    public void OnPointerClick(PointerEventData eventData) {
        Selected?.Invoke(this);
    }
}