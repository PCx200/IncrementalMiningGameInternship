using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrapBinDropTarget : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private EquipmentInventory equipmentInventory;

    [SerializeField]
    private EquipmentLoadout equipmentLoadout;

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

        Equipment scrappedEquipment = null;

        EquipmentInventoryDropTarget inventorySlot = eventData.pointerDrag.GetComponent<EquipmentInventoryDropTarget>();

        if (inventorySlot != null)
        {
            scrappedEquipment = draggedItemView.Equipment;

            if (!equipmentInventory.Remove(scrappedEquipment))
            {
                return;
            }

            OnEquipmentScrapped?.Invoke(scrappedEquipment);
            return;
        }

        EquipmentSlot loadoutSlot = eventData.pointerDrag.GetComponent<EquipmentSlot>();

        if (loadoutSlot != null)
        {
            scrappedEquipment = equipmentLoadout.TakeEquipped(loadoutSlot.SlotType);

            if (scrappedEquipment == null)
            {
                return;
            }

            OnEquipmentScrapped?.Invoke(scrappedEquipment);
        }
    }
}
