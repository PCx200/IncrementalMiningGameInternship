using UnityEngine;
using UnityEngine.InputSystem;

public class GodMode : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private float defaultDamage;

    [SerializeField]
    private float defaultAS;

    [SerializeField]
    private bool isON;

    private void Start()
    {

        defaultDamage = playerData.drillData.AttackDamage;
        defaultAS = playerData.drillData.AttackSpeed;
    }

    private void Update()
    {
        EnableGodMode();

        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            ShowCollectedBlocks();
        }
    }

    private void OnDisable()
    {
        playerData.drillData.AttackDamage = defaultDamage;
        playerData.drillData.AttackSpeed = defaultAS;
    }

    private void EnableGodMode()
    {
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            isON = !isON;
        }

        if (isON)
        {
            playerData.drillData.AttackDamage = 1000;
            playerData.drillData.AttackSpeed = 0.01f;
        }

        else
        {
            playerData.drillData.AttackDamage = defaultDamage;
            playerData.drillData.AttackSpeed = defaultAS;
        }
    }

    private void ShowCollectedBlocks()
    {
        foreach (var block in inventory.GetBlocks())
        {
            Debug.Log($"{block.Key.name} x{block.Value}");
        }
    }
}
