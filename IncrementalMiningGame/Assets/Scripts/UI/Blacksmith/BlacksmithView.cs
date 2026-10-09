using UnityEngine;

public class BlacksmithView : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    private EquipmentManager equipmentManager;

    [Header("UI")]
    [SerializeField]
    private GameObject mainPanel;

    private void Awake()
    {
        mainPanel.SetActive(false);
    }

    private void OnEnable()
    {
        equipmentManager.OnEquipmentChanged += Refresh;
    }

    private void OnDisable()
    {
        equipmentManager.OnEquipmentChanged -= Refresh;
    }

    public void Open()
    {
        Refresh();

        mainPanel.SetActive(true);
    }

    public void Close()
    {
        mainPanel.SetActive(false);
    }

    public void Refresh()
    {
        RefreshInventory();
        RefreshLoadout();
    }

    private void RefreshInventory()
    {
        for (int i = 0; i < equipmentManager.Capacity; i++)
        {
            EquipmentItemView itemView = equipmentManager.GetItemView(i);

            itemView.SetEquipment(equipmentManager.GetItem(i));
        }
    }

    private void RefreshLoadout()
    {
        foreach (EquipmentSlot slot in equipmentManager.EquipmentSlots)
        {
            EquipmentItemView itemView = slot.GetComponent<EquipmentItemView>();

            itemView.SetEquipment(slot.EquippedItem);
        }
    }
}