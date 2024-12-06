using UnityEngine;

public class MiniGame1_SoundManager : MonoBehaviour {
	[SerializeField] private AudioClip soundSuccess;
	[SerializeField] private AudioClip soundFail;
	[SerializeField] private AudioClip soundTouch;

	private AudioSource _audioSource;
	
	AudioSource audioSource {
		get {
			if (_audioSource == null)
				_audioSource = FindObjectOfType<AudioSource>();

			return _audioSource;
		}
	}

	public void PlaySuccess() {
		audioSource.PlayOneShot(soundSuccess, 1f);
	}

	public void PlayFail() {
		audioSource.PlayOneShot(soundFail, 1f);
	}

	public void PlayTouch() {
		audioSource.PlayOneShot(soundTouch, 1f);
	}
}