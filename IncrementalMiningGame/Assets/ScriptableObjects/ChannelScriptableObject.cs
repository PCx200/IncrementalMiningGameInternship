using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Channel", menuName = "Scriptable Objects/Channel")]
public abstract class ChannelScriptableObject<T> : ScriptableObject
{
    public event Action<T> Raised;

    public void Raise(T data)
    {
        Raised?.Invoke(data);
    }
}
