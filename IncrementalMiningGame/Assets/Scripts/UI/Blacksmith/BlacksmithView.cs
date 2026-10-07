using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithView : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    private EquipmentInventory equipmentInventory;

    [SerializeField]
    private EquipmentLoadout equipmentLoadout;

    [Header("UI")]
    [SerializeField]
    private GameObject mainPanel;

    private void Awake()
    {
        mainPanel.SetActive(false);
    }

    private void OnEnable()
    {
        equipmentInventory.OnInventoryChanged += HandleInventoryChanged;
        equipmentLoadout.OnLoadoutChanged += HandleLoadoutChanged;
    }

    private void OnDisable()
    {
        equipmentInventory.OnInventoryChanged -= HandleInventoryChanged;
        equipmentLoadout.OnLoadoutChanged -= HandleLoadoutChanged;
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

    private void HandleInventoryChanged()
    {
        if (!mainPanel.activeSelf)
        {
            return;
        }

        RefreshInventory();
    }

    private void HandleLoadoutChanged()
    {
        if (!mainPanel.activeSelf)
        {
            return;
        }

        RefreshLoadout();
    }

    private void RefreshInventory()
    {
        for (int i = 0; i < equipmentInventory.Capacity; i++)
        {
            EquipmentItemView itemView = equipmentInventory.GetItemView(i);

            if (itemView == null)
            {
                continue;
            }

            Equipment equipment = equipmentInventory.GetItem(i);

            if (equipment == null)
            {
                itemView.Clear();
                continue;
            }

            itemView.SetEquipment(equipment);
        }
    }

    private void RefreshLoadout()
    {
        foreach (EquipmentSlot slot in equipmentLoadout.EquipmentSlots)
        {
            EquipmentItemView itemView = slot.GetComponent<EquipmentItemView>();

            if (itemView == null)
            {
                continue;
            }

            Equipment equipment = slot.EquippedItem;

            if (equipment == null)
            {
                itemView.Clear();
                continue;
            }

            itemView.SetEquipment(equipment);
        }
    }
}