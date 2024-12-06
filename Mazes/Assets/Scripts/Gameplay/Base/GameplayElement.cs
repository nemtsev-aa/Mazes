using UnityEngine;

public class GameplayElement : MonoBehaviour {
    public MazeCell ParentMazeCell { get; private set; }

    public virtual void Init() {

    }

    public void SetParentMazeCell(MazeCell cell) {
        ParentMazeCell = cell;
    }
}
