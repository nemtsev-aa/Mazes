using System.Collections.Generic;
using UnityEngine;

public class DialInDisk : MonoBehaviour {
    [SerializeField] private int _charCount = 8;
    [SerializeField] private CharInDisk _charInDiskPrefab;

    private List<CharInDisk> _charInDiskList;

    public IReadOnlyList<CharInDisk> CharInDiskList => _charInDiskList;

    public void Init() {
        _charInDiskList = new List<CharInDisk>();

        CreateCharList();
    }

    private void CreateCharList() {
        int angleOfRotation = (int)(-360 / _charCount);

        for (int i = 0; i < _charCount; i++) {
            CharInPasswordConfig config = new CharInPasswordConfig(i, UnityEngine.Random.Range(1, 9));

            CharInDisk newChar = Instantiate(_charInDiskPrefab, transform);
            newChar.Init(config);
            newChar.gameObject.name = $"CharInDisk ({i}) {config.Value}";
            
            newChar.transform.eulerAngles = new Vector3(0, 0, angleOfRotation * i);
            newChar.LabelTransform.localEulerAngles = new Vector3(0, 0, -angleOfRotation * i);

            _charInDiskList.Add(newChar);
        }
    }
}
