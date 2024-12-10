using TMPro;
using UnityEngine;

public class CharInPasswordConfig {
    public CharInPasswordConfig(int index, int value) {
        Index = index;
        Value = value;
    }

    public int Index { get; private set; }
    public int Value { get; private set; }
}

public class CharInPassword : MonoBehaviour {
    [SerializeField] private float _selectedFontSize = 70f;
    [SerializeField] private float _unSelectedFontSize = 50f;
    [SerializeField] private Color _succesColor = Color.green;
    [SerializeField] private Color failColor = Color.red;

    [SerializeField] private TextMeshProUGUI _label;

    public CharInPasswordConfig Parameters { get; private set; }

    public void Init(CharInPasswordConfig parameters) {
        Parameters = parameters;

        _label.text = $"{Parameters.Value}";
        _label.fontSize = _unSelectedFontSize;
    }

    public void Select() {
        _label.fontSize = _selectedFontSize;
    }

    public void ShowResult(bool result) {
        if (result)
            _label.color = _succesColor;
        else
            _label.color = failColor;
    }

}

