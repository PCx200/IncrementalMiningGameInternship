using System.Collections.Generic;
using UnityEngine;

public class InventorySlotManager : MonoBehaviour
{
    [SerializeField]
    private Transform inventorySlotsRoot;

    private readonly List<EquipmentItemView> itemViews = new();
    private readonly List<EquipmentDropTarget> dropTargets = new();

    public int InventorySlotCount => itemViews.Count;

    public void Initialize()
    {
        itemViews.Clear();
        dropTargets.Clear();

        for (int i = 0; i < inventorySlotsRoot.childCount; i++)
        {
            Transform slot = inventorySlotsRoot.GetChild(i);

            EquipmentItemView itemView = slot.GetComponent<EquipmentItemView>();

            EquipmentDropTarget dropTarget = slot.GetComponent<EquipmentDropTarget>();

            itemViews.Add(itemView);
            dropTargets.Add(dropTarget);

            dropTarget.Initialize(i);
        }
    }

    public EquipmentItemView GetInventoryItemViewByIndex(int index)
    {
        return itemViews[index];
    }
}