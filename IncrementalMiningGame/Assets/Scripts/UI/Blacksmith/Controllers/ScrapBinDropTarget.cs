using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrapBinDropTarget : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private EquipmentManager equipmentManager;

    public event Action<Equipment> OnEquipmentScrapped;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        EquipmentItemView draggedItemView = eventData.pointerDrag.GetComponent<EquipmentItemView>();

        if (draggedItemView == null || draggedItemView.Equipment == null)
        {
            return;
        }

        Equipment equipment = draggedItemView.Equipment;

        EquipmentSlot loadoutSlot = eventData.pointerDrag.GetComponent<EquipmentSlot>();

        if (loadoutSlot != null)
        {
            equipment = equipmentManager.TakeEquipped(loadoutSlot.SlotType);

            if (equipment == null)
            {
                return;
            }

            OnEquipmentScrapped?.Invoke(equipment);

            return;
        }

        EquipmentDropTarget inventorySlot = eventData.pointerDrag.GetComponent<EquipmentDropTarget>();

        if (inventorySlot == null)
        {
            return;
        }

        if (!equipmentManager.Remove(equipment))
        {
            return;
        }

        OnEquipmentScrapped?.Invoke(equipment);
    }
}