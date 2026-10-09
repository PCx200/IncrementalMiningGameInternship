using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentDropTarget : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private EquipmentManager equipmentManager;

    private EquipmentSlot equipmentSlot;

    private int inventoryIndex = -1;

    public int InventoryIndex => inventoryIndex;

    private void Awake()
    {
        equipmentSlot = GetComponent<EquipmentSlot>();
    }

    public void Initialize(int index)
    {
        inventoryIndex = index;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        EquipmentItemView draggedItemView = eventData.pointerDrag.GetComponent<EquipmentItemView>();

        if (draggedItemView == null)
        {
            return;
        }

        Equipment equipment = draggedItemView.Equipment;

        if (equipment == null)
        {
            return;
        }

        if (equipmentSlot != null)
        {
            DropOnLoadout(equipment);
            return;
        }

        DropOnInventory(eventData);
    }

    private void DropOnLoadout(Equipment equipment)
    {
        if (equipment.Data.SlotType != equipmentSlot.SlotType)
        {
            return;
        }

        equipmentManager.TryEquip(equipment, equipmentSlot);
    }

    private void DropOnInventory(PointerEventData eventData)
    {
        EquipmentDropTarget sourceInventorySlot = eventData.pointerDrag.GetComponent<EquipmentDropTarget>();

        if (sourceInventorySlot != null && sourceInventorySlot.InventoryIndex >= 0)
        {
            equipmentManager.MoveOrSwap(sourceInventorySlot.InventoryIndex, inventoryIndex);

            return;
        }

        EquipmentSlot sourceLoadoutSlot = eventData.pointerDrag.GetComponent<EquipmentSlot>();

        if (sourceLoadoutSlot == null)
        {
            return;
        }

        if (equipmentManager.GetItem(inventoryIndex) != null)
        {
            return;
        }

        Equipment equipment = equipmentManager.TakeEquipped(sourceLoadoutSlot.SlotType);

        if (equipment == null)
        {
            return;
        }

        equipmentManager.TryAddAt(equipment, inventoryIndex);
    }
}