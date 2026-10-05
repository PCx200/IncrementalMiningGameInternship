using System;
using UnityEngine;

public abstract class ChannelScriptableObject<T> : ScriptableObject
{
    public event Action<T> Raised;

    public void Raise(T data)
    {
        Raised?.Invoke(data);
    }
}
