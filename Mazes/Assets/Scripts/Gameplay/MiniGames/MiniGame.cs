using System;
using UnityEngine;

public abstract class MiniGame : MonoBehaviour, IDisposable {
    public event Action<bool> Finished;

    protected bool IsActive;
    protected bool Result;

    public MiniGameConfig Config { get; private set; }

    public virtual void Init(MiniGameConfig config) {
        Config = config;
    }

    public virtual void StartGame() {
        IsActive = true;
    }

    public virtual void FinishGame() {
        gameObject.SetActive(false);
         
        Finished?.Invoke(Result);
    }

    public virtual void Dispose() {
        Destroy(gameObject);
    }
}

