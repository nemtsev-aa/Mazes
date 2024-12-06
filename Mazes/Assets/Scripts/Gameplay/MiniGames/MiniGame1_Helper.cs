using UnityEngine;

public class MiniGame1_Helper : MonoBehaviour {
	private MiniGame1_Mediator _gameMediator;
	private MiniGame1_Player _player;
	private MiniGame1_DotPosition _dotPosition;
	private MiniGame1_SoundManager _soundManager;
	private MiniGame1_ColorManager _colorManager;

	public MiniGame1_Player MiniGame1_Player {
		get {
			if (_player == null)
				_player = FindObjectOfType<MiniGame1_Player>();

			return _player;
		}
	}

	public MiniGame1_Mediator MiniGame1_Mediator {
		get {
			if (_gameMediator == null)
				_gameMediator = FindObjectOfType<MiniGame1_Mediator>();

			return _gameMediator;
		}
	}

	public MiniGame1_DotPosition DotPosition {
		get {
			if (_dotPosition == null)
				_dotPosition = FindObjectOfType<MiniGame1_DotPosition>();

			return _dotPosition;
		}
	}

	public MiniGame1_SoundManager SoundManager {
		get {
			if (_soundManager == null)
				_soundManager = FindObjectOfType<MiniGame1_SoundManager>();

			return _soundManager;
		}
	}

	public MiniGame1_ColorManager ColorManager {
		get {
			if (_colorManager == null)
				_colorManager = FindObjectOfType<MiniGame1_ColorManager>();

			return _colorManager;
		}
	}
}
