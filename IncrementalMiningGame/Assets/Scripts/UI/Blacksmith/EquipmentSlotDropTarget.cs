using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotDropTarget : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private EquipmentLoadout equipmentLoadout;

    private EquipmentSlot equipmentSlot;

    private void Awake()
    {
        equipmentSlot = GetComponent<EquipmentSlot>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        EquipmentItemView draggedItemView = eventData.pointerDrag?.GetComponent<EquipmentItemView>();

        if (draggedItemView == null)
        {
            return;
        }

        Equipment equipment = draggedItemView.Equipment;

        if (equipment == null || equipment.Data == null)
        {
            return;
        }

        if (equipment.Data.SlotType != equipmentSlot.SlotType)
        {
            return;
        }

        equipmentLoadout.TryEquip(equipment);
    }
}