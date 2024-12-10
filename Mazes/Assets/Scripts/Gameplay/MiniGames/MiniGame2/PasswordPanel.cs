using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PasswordPanel : MonoBehaviour {
    public event Action CharsIsOver;

    [SerializeField] private CharInPassword _charPrefab;

    private List<CharInPassword> _charList;
    private MiniGame2 _miniGame;
    private int _charCount;

    private List<CharInDisk> _charInDiskList;
    private CharInPassword _currentCharInPassword;

    public void Init(MiniGame2 miniGame, int charCount, IReadOnlyList<CharInDisk> charInDiskList) {
        _miniGame = miniGame;
        _miniGame.MiniGame2_IsStarted += SwitchToNextChar;

        _charCount = charCount;

        _charInDiskList = new List<CharInDisk>();
        _charInDiskList.AddRange(charInDiskList);

        _charList = new List<CharInPassword>();

        CreateCharList();
    }

    private void SwitchToNextChar() {
        if (_currentCharInPassword == null) {
            _currentCharInPassword = _charList[0];
            _currentCharInPassword.Select();
            return;
        }

        if (_currentCharInPassword != null) {
            var nextChar = _charList.FirstOrDefault(c => c.Parameters.Index == _currentCharInPassword.Parameters.Index + 1);

            if (nextChar != null) {
                _currentCharInPassword = nextChar;
                _currentCharInPassword.Select();
                return;
            }

            CharsIsOver?.Invoke();
        }
    }

    private void CreateCharList() {
        for (int i = 0; i < _charCount; i++) {
            int randomIndex = UnityEngine.Random.Range(0, _charInDiskList.Count);
            int value = _charInDiskList[randomIndex].Parameters.Value;

            CharInPasswordConfig config = new CharInPasswordConfig(i, value);

            CharInPassword newChar = Instantiate(_charPrefab, transform);
            newChar.Init(config);

            _charList.Add(newChar);
            _charInDiskList.Remove(_charInDiskList[randomIndex]);
        }
    }

    public bool CheckCharInPassword(int value) {

        if (_currentCharInPassword.Parameters.Value == value) {
            _currentCharInPassword.ShowResult(true);
            SwitchToNextChar();
            return true;
        }

        _currentCharInPassword.ShowResult(false);
        return false;
    }


}
