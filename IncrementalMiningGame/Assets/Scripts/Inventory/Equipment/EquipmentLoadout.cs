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
        //drillController = FindFirstObjectByType<DrillController>();
    }

    public bool TryEquip(Equipment equipment)
    {
        if (equipment == null || equipment.Data == null)
        {
            return false;
        }

        if (!equipmentInventory.Contains(equipment))
        {
            return false;
        }

        foreach (EquipmentSlot slot in equipmentSlots)
        {
            if (slot.SlotType != equipment.Data.SlotType)
            {
                continue;
            }

            Equipment previouslyEquipped = slot.EquippedItem;

            if (!slot.TryEquip(equipment))
            {
                return false;
            }

            if (!equipmentInventory.Remove(equipment))
            {
                return false;
            }

            if (previouslyEquipped != null)
            {
                equipmentInventory.TryAdd(previouslyEquipped);
            }

            RebuildEquipmentModifiers();

            OnLoadoutChanged?.Invoke();

            return true;
        }

        return false;
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

            if (equipmentInventory.IsFull())
            {
                return false;
            }

            slot.Unequip();

            if (!equipmentInventory.TryAdd(equipment))
            {
                return false;
            }

            RebuildEquipmentModifiers();

            OnLoadoutChanged?.Invoke();

            return true;
        }

        return false;
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

            foreach (DrillStatModifier secondaryModifier in equipment.SecondaryModifiers)
            {
                modifiers.Add(secondaryModifier);
            }
        }

        drillController.DrillStatManager.SetModifiers(DrillModifierSource.Equipment, modifiers);
    }
}
