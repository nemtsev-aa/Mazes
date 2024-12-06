using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargeIcon : MonoBehaviour {
    [SerializeField] private GameObject _resetView;
    [SerializeField] private GameObject _activeView;

    [SerializeField] private Image _foreground;
    [SerializeField] private TextMeshProUGUI _text;

    public void StartCharge() {
        _resetView.SetActive(true);
        _activeView.SetActive(false);
    }

    public void StopCharge() {
        _resetView.SetActive(false);
        _activeView.SetActive(true);
    }

    public void SetChargeValue(float currentCharge, float maxCharge) {
        _foreground.fillAmount = currentCharge / maxCharge;
        _text.text = Mathf.Ceil(maxCharge - currentCharge).ToString();
    }
}
