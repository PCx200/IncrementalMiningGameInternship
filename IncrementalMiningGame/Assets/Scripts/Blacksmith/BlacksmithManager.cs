using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithManager : MonoBehaviour
{
    [SerializeField]
    private EquipmentInventory equipmentInventory;

    [SerializeField]
    private List<EquipmentData> equipmentDataPool;

    [SerializeField]
    private ForgedEquipmentView forgedEquipmentView;

    [SerializeField]
    private float forgeDisplayDuration;

    private Equipment forgedEquipment;
    private bool isForging;

    public void ForgeEquipment()
    {
        if (isForging)
        {
            return;
        }

        if (equipmentInventory.IsFull())
        {
            return;
        }

        int randomEquipmentIndex = Random.Range(0, equipmentDataPool.Count);

        EquipmentData pickedEquipmentData = equipmentDataPool[randomEquipmentIndex];

        forgedEquipment = EquipmentForager.Forge(pickedEquipmentData);

        forgedEquipmentView.Show(forgedEquipment);

        isForging = true;

        StartCoroutine(FinishForge());
    }

    private IEnumerator FinishForge()
    {
        yield return new WaitForSeconds(forgeDisplayDuration);

        equipmentInventory.TryAdd(forgedEquipment);

        forgedEquipment = null;

        forgedEquipmentView.Clear();

        isForging = false;
    }
}