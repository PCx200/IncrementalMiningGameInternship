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

    public int Count => items.Count;

    public event Action OnInventoryChanged;

    private void Start()
    {
        for (int i = 0; i < capacity; i++)
        {
            EquipmentItemView equipment = transform.GetChild(i).GetComponent<EquipmentItemView>();
            itemViews.Add(equipment);
        }
    }

    public bool TryAdd(Equipment item)
    {
        if (item == null)
        {
            return false;
        }

        if (IsFull())
        {
            return false;
        }

        items.Add(item);

        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool Remove(Equipment item)
    {
        if (item == null)
        {
            return false;
        }

        bool removed = items.Remove(item);

        if (!removed)
        {
            return false;
        }

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
        return items.Count >= capacity;
    }

    public void Clear()
    {
        if (items.Count == 0)
        {
            return;
        }

        items.Clear();

        OnInventoryChanged?.Invoke();
    }
}