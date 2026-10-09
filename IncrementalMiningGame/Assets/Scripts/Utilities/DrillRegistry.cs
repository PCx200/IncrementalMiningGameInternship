using System;
using UnityEngine;

public class DrillRegistry : MonoBehaviour
{
    public static DrillRegistry Instance { get; private set; }

    public DrillController CurrentDrill { get; private set; }

    public event Action<DrillController> OnDrillRegistered;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Register(DrillController drill)
    {
        CurrentDrill = drill;

        OnDrillRegistered?.Invoke(drill);
    }

    public void Unregister(DrillController drill)
    {
        if (CurrentDrill != drill)
        {
            return;
        }

        CurrentDrill = null;
    }
}