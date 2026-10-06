using UnityEngine;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField]
    private SlotType slotType;
    public SlotType SlotType => slotType;

    private Equipment equippedItem;
    public Equipment EquippedItem => equippedItem;

    public bool TryEquip(Equipment item)
    {
        if (item == null || item.Data == null)
        {
            return false;
        }

        if (item.Data.SlotType != slotType)
        {
            return false;
        }

        equippedItem = item;

        return true;
    }

    public Equipment Unequip()
    {
        Equipment removedItem = equippedItem;

        equippedItem = null;

        return removedItem;
    }
}
