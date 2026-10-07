using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentInventoryDropTarget : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private EquipmentLoadout equipmentLoadout;

    [SerializeField]
    private EquipmentInventory equipmentInventory;

    private int inventoryIndex;
    public int InventoryIndex => inventoryIndex;

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

        EquipmentInventoryDropTarget sourceInventorySlot = eventData.pointerDrag.GetComponent<EquipmentInventoryDropTarget>();

        if (sourceInventorySlot != null)
        {
            equipmentInventory.MoveOrSwap(sourceInventorySlot.InventoryIndex, inventoryIndex);

            return;
        }

        EquipmentSlot sourceLoadoutSlot = eventData.pointerDrag.GetComponent<EquipmentSlot>();

        if (sourceLoadoutSlot == null)
        {
            return;
        }

        if (equipmentInventory.GetItem(inventoryIndex) != null)
        {
            return;
        }

        Equipment equipment = equipmentLoadout.TakeEquipped(sourceLoadoutSlot.SlotType);

        if (equipment == null)
        {
            return;
        }

        equipmentInventory.TryAddAt(equipment, inventoryIndex);
    }
}