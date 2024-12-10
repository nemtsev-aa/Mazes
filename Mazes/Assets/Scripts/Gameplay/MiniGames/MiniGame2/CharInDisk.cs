using TMPro;
using UnityEngine;

public class CharInDisk : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _label;

    [SerializeField] public Transform LabelTransform;

    public CharInPasswordConfig Parameters { get; private set; }

    public void Init(CharInPasswordConfig parameters) {
        Parameters = parameters;

        _label.text = $"{Parameters.Value}";
    }
}
