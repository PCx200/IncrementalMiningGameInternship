using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollViewZoom : MonoBehaviour, IScrollHandler
{
    [SerializeField]
    private RectTransform content;
    [SerializeField] 
    private RectTransform viewport;

    [SerializeField] 
    private float zoomSpeed;
    [SerializeField] 
    private float minZoom;
    [SerializeField] 
    private float maxZoom;

    public void OnScroll(PointerEventData eventData)
    {
        if (eventData.scrollDelta.y == 0)
        { 
            return;
        }

        // Mouse position in viewport-local coordinates
        RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, eventData.position,
            eventData.pressEventCamera, out Vector2 mousePosition
        );

        float oldScale = content.localScale.x;

        // Calculate new scale
        float zoomFactor = 1f + eventData.scrollDelta.y * zoomSpeed;
        float newScale = Mathf.Clamp(oldScale * zoomFactor, minZoom, maxZoom);

        // Don't do anything if we are already at the limit
        if (Mathf.Approximately(oldScale, newScale))
        { 
            return;
        }

        // Position of the mouse relative to the Content
        Vector2 contentPositionBefore = content.anchoredPosition;

        // How much the content needs to move to keep the mouse over the same point in the content
        Vector2 offsetFromContent = mousePosition - contentPositionBefore;

        float scaleRatio = newScale / oldScale;

        content.localScale = new Vector3(newScale, newScale, 1f);

        content.anchoredPosition = mousePosition - offsetFromContent * scaleRatio;
    }
}