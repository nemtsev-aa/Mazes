using UnityEngine;
using System;
using DG.Tweening;

public class MiniGame1_DotPosition : MiniGame1_Helper {
	[SerializeField] private Transform _dotTransform;
	[SerializeField] private RectTransform _dotRectTransform;

	public float DotSize => _dotRectTransform.sizeDelta.x;
	public Transform DotTransform => _dotTransform;
	public bool IsLeftOfScreen => transform.eulerAngles.z >= 0 && transform.eulerAngles.z < 180;
	
	public void SetNewPosition() {
		float minRotDecal = 30;

		if (UnityEngine.Random.Range(0, 2) == 0)
			minRotDecal *= -1;

		float randLeft = UnityEngine.Random.Range(-90.00f, -30.00f);
		float randRight = UnityEngine.Random.Range(30.00f, 90.00f);
		float rotationTemp = randLeft;

		if (UnityEngine.Random.Range(0, 2) == 0)
			rotationTemp = randRight;

		float rotation = MiniGame1_Player.Rotation + rotationTemp;

		transform.eulerAngles = Vector3.forward * rotation;

		_dotTransform.localScale = Vector2.zero;
		
		DoScale(0, 1, () => {
		});
	}



	private void DoScale(float s0, float s1, Action callback) {
		
		Sequence scaling = DOTween.Sequence();
		scaling.Append(_dotTransform.DOScale(Vector2.one * s0, 0))
			   .Append(_dotTransform.DOScale(Vector2.one * s1, 0.2f))
			   .OnComplete( () => {
				   if (callback != null)
					   callback();
			   });
	}
}
