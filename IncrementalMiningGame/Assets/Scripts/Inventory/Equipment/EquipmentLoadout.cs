using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentLoadout : MonoBehaviour
{
    [SerializeField]
    private DrillController drillController;

    [SerializeField]
    private List<EquipmentSlot> equipmentSlots;

    public List<EquipmentSlot> EquipmentSlots => equipmentSlots;

    [SerializeField]
    private EquipmentInventory equipmentInventory;

    public event Action OnLoadoutChanged;

    private void Start()
    {
        drillController = FindFirstObjectByType<DrillController>();
    }

    public bool TryEquip(Equipment equipment, EquipmentSlot slot)
    {
        if (equipment == null || slot == null)
        {
            return false;
        }

        if (!equipmentInventory.Contains(equipment))
        {
            return false;
        }

        Equipment previouslyEquipped = slot.EquippedItem;

        if (!slot.TryEquip(equipment))
        {
            return false;
        }

        equipmentInventory.Remove(equipment);

        if (previouslyEquipped != null)
        {
            equipmentInventory.TryAdd(previouslyEquipped);
        }

        RebuildEquipmentModifiers();

        OnLoadoutChanged?.Invoke();

        return true;
    }

    public bool Unequip(SlotType slotType)
    {
        foreach (EquipmentSlot slot in equipmentSlots)
        {
            if (slot.SlotType != slotType)
            {
                continue;
            }

            Equipment equipment = slot.EquippedItem;

            if (equipment == null)
            {
                return false;
            }

            slot.Unequip();
            equipmentInventory.TryAdd(equipment);

            RebuildEquipmentModifiers();

            OnLoadoutChanged?.Invoke();

            return true;
        }

        return false;
    }

    public Equipment TakeEquipped(SlotType slotType)
    {
        foreach (EquipmentSlot slot in equipmentSlots)
        {
            if (slot.SlotType != slotType)
            {
                continue;
            }

            Equipment equipment = slot.Unequip();

            if (equipment == null)
            {
                return null;
            }

            RebuildEquipmentModifiers();

            OnLoadoutChanged?.Invoke();

            return equipment;
        }

        return null;
    }

    private void RebuildEquipmentModifiers()
    {
        List<DrillStatModifier> modifiers = new();

        foreach (EquipmentSlot slot in equipmentSlots)
        {
            Equipment equipment = slot.EquippedItem;

            if (equipment == null)
            {
                continue;
            }

            modifiers.Add(equipment.PrimaryModifier);

            modifiers.AddRange(equipment.SecondaryModifiers);
        }

        drillController.DrillStatManager.SetModifiers(DrillModifierSource.Equipment, modifiers);
    }
}
