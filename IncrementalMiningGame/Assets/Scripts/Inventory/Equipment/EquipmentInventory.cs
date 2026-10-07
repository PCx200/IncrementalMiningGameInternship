using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventory : MonoBehaviour
{
    [SerializeField]
    private int capacity = 24;
    public int Capacity => capacity;

    private readonly List<Equipment> items = new();
    public IReadOnlyList<Equipment> Items => items;

    private readonly List<EquipmentItemView> itemViews = new();
    public IReadOnlyList<EquipmentItemView> ItemViews => itemViews;

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

    public event Action OnInventoryChanged;

    //TEMP
    [SerializeField]
    private EquipmentData equipmentData;
    [SerializeField]
    private EquipmentData equipmentData1;

    private void Awake()
    {
        for (int i = 0; i < capacity; i++)
        {
            Transform slotTransform = transform.GetChild(i);

            EquipmentItemView itemView = slotTransform.GetComponent<EquipmentItemView>();

            itemViews.Add(itemView);

            EquipmentInventoryDropTarget dropTarget = slotTransform.GetComponent<EquipmentInventoryDropTarget>();

            if (dropTarget != null)
            {
                dropTarget.Initialize(i);
            }

            items.Add(null);
        }
    }

    private void Start()
    {
        //TEMP
        TryAdd(new Equipment(equipmentData));
        TryAdd(new Equipment(equipmentData1));
    }

    public bool TryAdd(Equipment item)
    {
        if (item == null)
        {
            return false;
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                continue;
            }

            items[i] = item;

            OnInventoryChanged?.Invoke();

            return true;
        }

        return false;
    }

    public bool TryAddAt(Equipment item, int index)
    {
        if (item == null)
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

        items[index] = item;

        OnInventoryChanged?.Invoke();

        return true;
    }
    public bool MoveOrSwap(int sourceIndex, int targetIndex)
    {
        if (sourceIndex < 0 || sourceIndex >= capacity)
        {
            return false;
        }

        if (targetIndex < 0 || targetIndex >= capacity)
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

        OnInventoryChanged?.Invoke();

        return true;
    }


    public bool Remove(Equipment item)
    {
        if (item == null)
        {
            return false;
        }

        int index = items.IndexOf(item);

        if (index < 0)
        {
            return false;
        }

        items[index] = null;

        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool Contains(Equipment item)
    {
        return items.Contains(item);
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
        if (index < 0 || index >= itemViews.Count)
        {
            return null;
        }

        return itemViews[index];
    }

    public bool IsFull()
    {
        foreach (Equipment item in items)
        {
            if (item == null)
            {
                return false;
            }
        }

        return true;
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
            OnInventoryChanged?.Invoke();
        }
    }
}