using System;
using System.Collections;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    [SerializeField]
    private DrillController drill;

    private PlayerController player;
    [SerializeField]
    private Inventory inventory;

    public event Action OnRoundEnd;

    private bool hasRoundEnded = false;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void RegisterDrill(DrillController newDrill)
    {
        hasRoundEnded = false;

        if (drill != null)
        {
            drill.OnTankEmpty -= RoundEnd;
        }

        drill = newDrill;
        drill.OnTankEmpty += RoundEnd;
    }

    private void OnDisable()
    {
        if (drill != null)
        {
            drill.OnTankEmpty -= RoundEnd;
        }
    }

    private void RoundEnd()
    {
        if (hasRoundEnded)
        {
            return;
        }

        hasRoundEnded = true;

        DisablePlayer();

        OnRoundEnd?.Invoke();
    }

    private void DisablePlayer()
    {
        player = FindFirstObjectByType<PlayerController>();
        player.enabled = false;

        inventory = FindFirstObjectByType<Inventory>();
        inventory.enabled = false;
    }
}
