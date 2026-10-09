using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentDropTarget : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private EquipmentManager equipmentManager;

    private int inventoryIndex = -1;

    public int InventoryIndex => inventoryIndex;

    private EquipmentSlot equipmentSlot;

    private void Awake()
    {
        equipmentSlot = GetComponentInParent<EquipmentSlot>();
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

        // INVENTORY -> LOADOUT
        if (equipmentSlot != null)
        {
            if (equipment.Data.SlotType != equipmentSlot.SlotType)
            {
                return;
            }

            equipmentManager.TryEquip(equipment, equipmentSlot);

            return;
        }

        // LOADOUT -> INVENTORY
        EquipmentSlot sourceLoadoutSlot = eventData.pointerDrag.GetComponentInParent<EquipmentSlot>();

        if (sourceLoadoutSlot != null)
        {
            equipmentManager.MoveEquippedToInventory(sourceLoadoutSlot, inventoryIndex);

            return;
        }

        // INVENTORY -> INVENTORY
        EquipmentDropTarget sourceInventorySlot = eventData.pointerDrag.GetComponent<EquipmentDropTarget>();

        if (sourceInventorySlot == null)
        {
            return;
        }

        equipmentManager.MoveOrSwap(sourceInventorySlot.InventoryIndex, inventoryIndex);
    }
}