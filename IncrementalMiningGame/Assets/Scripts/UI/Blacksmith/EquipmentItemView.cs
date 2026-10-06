using UnityEngine;
using UnityEngine.UI;

public class EquipmentItemView : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;

    private Equipment equipment;
    public Equipment Equipment => equipment;

    public void SetEquipment(Equipment newEquipment)
    {
        equipment = newEquipment;

        Refresh();
    }

    public void Clear()
    {
        equipment = null;

        Refresh();
    }

    private void Refresh()
    {
        if (equipment == null || equipment.Data == null)
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;

            return;
        }

        itemIcon.sprite = equipment.Data.Icon;
        itemIcon.enabled = true;
    }
}