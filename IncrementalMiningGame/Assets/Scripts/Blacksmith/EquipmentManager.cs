using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField]
    private int capacity = 24;

    public int Capacity => capacity;

    private readonly List<Equipment> items = new();
    public IReadOnlyList<Equipment> Items => items;

    [SerializeField]
    private InventorySlotManager inventorySlotManager;

    [Header("Loadout")]
    [SerializeField]
    private List<EquipmentSlot> equipmentSlots;

    public IReadOnlyList<EquipmentSlot> EquipmentSlots => equipmentSlots;

    private DrillController drillController;

    public event Action OnEquipmentChanged;

    public int Count
    {
        get
        {
            int count = 0;

            foreach (Equipment item in items)
            {
                if (item != null)
                {
                    count++;
                }
            }

            return count;
        }
    }

    private void Awake()
    {
        inventorySlotManager.Initialize();

        for (int i = 0; i < inventorySlotManager.InventorySlotCount; i++)
        {
            items.Add(null);
        }
    }

    private void OnEnable()
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
    }

    private void RegisterDrill(DrillController drillController)
    {
        this.drillController = drillController;

        RebuildEquipmentModifiers();
    }

    /*
    * --------------------
    * INVENTORY
    * --------------------
    */

    public bool TryAdd(Equipment equipment)
    {
        if (equipment == null)
        {
            return false;
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                continue;
            }

            items[i] = equipment;

            OnEquipmentChanged?.Invoke();

            return true;
        }

        return false;
    }

    public bool TryAddAt(Equipment equipment, int index)
    {
        if (equipment == null)
        {
            return false;
        }

        if (index < 0 || index >= capacity)
        {
            return false;
        }

        if (items[index] != null)
        {
            return false;
        }

        items[index] = equipment;

        OnEquipmentChanged?.Invoke();

        return true;
    }

    public bool MoveOrSwap(int sourceIndex, int targetIndex)
    {
        if (sourceIndex < 0 || sourceIndex >= capacity || targetIndex < 0 || targetIndex >= capacity)
        {
            return false;
        }

        if (sourceIndex == targetIndex)
        {
            return false;
        }

        Equipment sourceEquipment = items[sourceIndex];

        if (sourceEquipment == null)
        {
            return false;
        }

        Equipment targetEquipment = items[targetIndex];

        items[targetIndex] = sourceEquipment;
        items[sourceIndex] = targetEquipment;

        OnEquipmentChanged?.Invoke();

        return true;
    }

    public bool MoveEquippedToInventory(EquipmentSlot sourceSlot, int inventoryIndex)
    {
        if (sourceSlot == null)
        {
            return false;
        }

        if (inventoryIndex < 0 || inventoryIndex >= Capacity)
        {
            return false;
        }

        Equipment equippedEquipment = sourceSlot.EquippedItem;

        if (equippedEquipment == null)
        {
            return false;
        }

        Equipment inventoryEquipment = items[inventoryIndex];

        if (inventoryEquipment != null)
        {
            if (inventoryEquipment.Data.SlotType != sourceSlot.SlotType)
            {
                return false;
            }

            if (!sourceSlot.TryEquip(inventoryEquipment))
            {
                return false;
            }

            items[inventoryIndex] = equippedEquipment;
        }
        else
        {
            sourceSlot.Unequip();

            items[inventoryIndex] = equippedEquipment;
        }

        RebuildEquipmentModifiers();

        OnEquipmentChanged?.Invoke();

        return true;
    }

    public bool Remove(Equipment equipment)
    {
        int index = items.IndexOf(equipment);

        if (index < 0)
        {
            return false;
        }

        items[index] = null;

        OnEquipmentChanged?.Invoke();

        return true;
    }

    public bool Contains(Equipment equipment)
    {
        return items.Contains(equipment);
    }

    public Equipment GetItem(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            return null;
        }

        return items[index];
    }

    public EquipmentItemView GetItemView(int index)
    {
        if (index < 0 || index >= Capacity)
        {
            return null;
        }

        return inventorySlotManager.GetInventoryItemViewByIndex(index);
    }

    public bool IsFull()
    {
        return capacity == Count;
    }

    public void Clear()
    {
        bool hadItems = false;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                hadItems = true;
            }

            items[i] = null;
        }

        if (hadItems)
        {
            OnEquipmentChanged?.Invoke();
        }
    }

    /*
    * --------------------
    * LOADOUT
    * --------------------
    */

    public bool TryEquip(Equipment equipment, EquipmentSlot slot)
    {
        if (equipment == null || slot == null)
        {
            return false;
        }

        if (!Contains(equipment))
        {
            return false;
        }

        Equipment previouslyEquipped = slot.EquippedItem;

        if (!slot.TryEquip(equipment))
        {
            return false;
        }

        int inventoryIndex = items.IndexOf(equipment);

        items[inventoryIndex] = previouslyEquipped;

        RebuildEquipmentModifiers();

        OnEquipmentChanged?.Invoke();

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

            if (equipment == null || IsFull())
            {
                return false;
            }

            slot.Unequip();

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null)
                {
                    continue;
                }

                items[i] = equipment;
                break;
            }

            RebuildEquipmentModifiers();

            OnEquipmentChanged?.Invoke();

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

            OnEquipmentChanged?.Invoke();

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