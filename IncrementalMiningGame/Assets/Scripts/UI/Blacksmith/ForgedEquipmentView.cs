using UnityEngine;
using UnityEngine.UI;

public class ForgedEquipmentView : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;

    public void Show(Equipment equipment)
    {
        itemImage.sprite = equipment.Data.Icon;
        itemImage.enabled = true;
    }

    public void Clear()
    {
        itemImage.sprite = null;
        itemImage.enabled = false;
    }
}
