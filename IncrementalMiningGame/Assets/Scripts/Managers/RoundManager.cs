using System;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    private DrillController drill;

    private PlayerController player;

    private ResourceBag inventory;

    public event Action OnRoundEnd;

    private bool hasRoundEnded = false;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
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
            drill.OnTankEmpty -= EndRound;
        }

        drill = newDrill;
        drill.OnTankEmpty += EndRound;
    }

    private void OnDisable()
    {
        if (drill != null)
        {
            drill.OnTankEmpty -= EndRound;
        }
    }

    private void EndRound()
    {
        if (hasRoundEnded)
        {
            return;
        }

        hasRoundEnded = true;

        DisableComponents();

        OnRoundEnd?.Invoke();
    }

    private void DisableComponents()
    {
        player = FindFirstObjectByType<PlayerController>();
        player.enabled = false;

        inventory = FindFirstObjectByType<ResourceBag>();
        inventory.enabled = false;
    }
}
