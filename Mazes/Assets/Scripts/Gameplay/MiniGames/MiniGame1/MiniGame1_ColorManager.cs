using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniGame1_ColorManager : MonoBehaviour {
	[SerializeField] private Color[] _colors;
	[SerializeField] private float _timeChangeColor = 10;

	[SerializeField] private Image _background;
	[SerializeField] private Image _clock;
	[SerializeField] private Image _lock;

	private Color _color;

	public void ChangeColor() {
		Color colorTemp = _colors[UnityEngine.Random.Range(0, _colors.Length)];

		StartCoroutine(DoLerp(_background.color, colorTemp, 1f));
	}

	void OnEnable() {
		_background.color = GetLastColor();
		UpdateCircleColor();
	}

	void OnDisable() {
		StopAllCoroutines();
		SaveLastColor();
		PlayerPrefs.Save();
	}

	private Color GetLastColor() {
		if (PlayerPrefs.HasKey("COLOR_R")) {
			float r = PlayerPrefs.GetFloat("COLOR_R");
			float g = PlayerPrefs.GetFloat("COLOR_G");
			float b = PlayerPrefs.GetFloat("COLOR_B");

			Color c = new Color(r, g, b, 1f);

			return c;
		}

		return _colors[0];
	}

	private void SaveLastColor() {
		Color c = _background.color;

		float r = c.r;
		float g = c.g;
		float b = c.b;

		PlayerPrefs.SetFloat("COLOR_R", r);
		PlayerPrefs.SetFloat("COLOR_G", g);
		PlayerPrefs.SetFloat("COLOR_B", b);
	}

	private IEnumerator DoLerp(Color from, Color to, float time) {
		float timer = 0;
		
		while (timer <= time) {
			timer += Time.deltaTime;
			_background.color = Color.Lerp(from, to, timer / time);
			SaveLastColor();
			UpdateCircleColor();
			yield return null;
		}

		_background.color = to;
		UpdateCircleColor();
		PlayerPrefs.Save();
	}

	private void UpdateCircleColor() {
		Color c = _background.color;

		Color temp = new Color(c.r / 2f, c.g / 2f, c.b / 2f, 1f);
		Color temp2 = new Color(c.r / 2f, c.g / 2f, c.b / 2f, 0.6f);

		_clock.color = temp;
		_lock.color = temp2;
	}

}
