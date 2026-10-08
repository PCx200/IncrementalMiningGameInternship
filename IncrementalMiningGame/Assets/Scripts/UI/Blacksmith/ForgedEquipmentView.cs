using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ForgedEquipmentView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Equipment equipment;
    public Equipment Equipment => equipment;

    [SerializeField]
    private Image itemImage;

    [Header("Descriptions")]
    [SerializeField]
    private EquipmentDescriptionView equipmentDescriptionView;

    private void Awake()
    {
        Clear();
    }

    public void Show(Equipment equipment)
    {
        this.equipment = equipment;
        itemImage.sprite = equipment.Data.Icon;
        itemImage.enabled = true;
    }

    public void Clear()
    {
        equipment = null;
        itemImage.sprite = null;
        itemImage.enabled = false;

        equipmentDescriptionView.Hide();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (equipment == null)
        {
            return;
        }

        equipmentDescriptionView.Show(equipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (equipment == null)
        {
            return;
        }

        equipmentDescriptionView.Hide();
    }
}
