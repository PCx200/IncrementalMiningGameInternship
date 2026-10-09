using System;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    private DrillController drill;

    private PlayerController player;

    private ResourceBag resourceBag;

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

    private void Start()
    {
        DrillRegistry.Instance.OnDrillRegistered += RegisterDrill;

        if (DrillRegistry.Instance.CurrentDrill != null)
        {
            RegisterDrill(DrillRegistry.Instance.CurrentDrill);
        }
    }

    private void OnDisable()
    {
        DrillRegistry.Instance.OnDrillRegistered -= RegisterDrill;

        if (drill != null)
        {
            drill.OnTankEmpty -= EndRound;
        }
    }

    private void RegisterDrill(DrillController newDrill)
    {
        if (drill != null)
        {
            drill.OnTankEmpty -= EndRound;
        }

        drill = newDrill;

        drill.OnTankEmpty += EndRound;

        hasRoundEnded = false;
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

        resourceBag = FindFirstObjectByType<ResourceBag>();
        resourceBag.enabled = false;
    }
}
