using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentItemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Equipment equipment;
    public Equipment Equipment => equipment;

    [SerializeField]
    private Image itemIcon;

    private GameObject dragIcon;

    [SerializeField]
    private RectTransform dragLayer;

    [Header("Descriptions")]
    [SerializeField]
    private EquipmentDescriptionView equipmentDescriptionView;

    [Header("Scrap Bin")]
    [SerializeField]
    private ScrapBinView scrapBinView;

    private void Awake()
    {
        Clear();
    }

    public void SetEquipment(Equipment equipment)
    {
        this.equipment = equipment;

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

    public void OnBeginDrag(PointerEventData eventData)
    {     
        if (equipment == null || dragLayer == null)
        {
            return;
        }

        dragIcon = new GameObject("EquipmentDragIcon", typeof(RectTransform), typeof(CanvasGroup), typeof(Image));

        dragIcon.transform.SetParent(dragLayer, false);

        Image dragImage = dragIcon.GetComponent<Image>();

        dragImage.sprite = equipment.Data.Icon;
        dragImage.raycastTarget = false;

        RectTransform dragRect = dragIcon.GetComponent<RectTransform>();

        dragRect.sizeDelta = itemIcon.rectTransform.rect.size;
        dragRect.position = eventData.position;

        CanvasGroup canvasGroup = dragIcon.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        scrapBinView.Show();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon == null)
        {
            return;
        }

        dragIcon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            Destroy(dragIcon);
            dragIcon = null;
        }

        scrapBinView.Hide();
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