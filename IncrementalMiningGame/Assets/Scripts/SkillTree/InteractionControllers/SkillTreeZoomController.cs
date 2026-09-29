using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTreeZoomController : MonoBehaviour, IScrollHandler
{
    private RectTransform zoomableTranform;

    [SerializeField] 
    private float zoomSpeed;

    [SerializeField] 
    private float minScale;

    [SerializeField]
    private float maxScale;

    private void Start()
    {
        zoomableTranform = GetComponent<RectTransform>();
    }

    public void OnScroll(PointerEventData eventData)
    {
        float scroll = eventData.scrollDelta.y;
        if (Mathf.Approximately(scroll, 0f))
        { 
            return;
        }

        float oldScale = zoomableTranform.localScale.x;
        float newScale = Mathf.Clamp(oldScale + scroll * zoomSpeed, minScale, maxScale);

        // Zoom toward mouse position
        Zoom(oldScale, newScale, eventData);
    }

    private void Zoom(float oldScale, float newScale, PointerEventData eventData)
    {
        RectTransform parent = zoomableTranform.parent as RectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parent,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 mouseLocal
        );

        Vector2 beforePos = zoomableTranform.localPosition;
        Vector2 fromTargetToMouse = mouseLocal - beforePos;

        float ratio = newScale / oldScale;

        // Scale visually
        zoomableTranform.localScale = new Vector3(newScale, newScale, 1f);

        // Scale RectTransform sizeDelta so logical size matches visual size
        zoomableTranform.sizeDelta /= ratio;

        // Reposition so zoom centers on cursor
        zoomableTranform.localPosition = mouseLocal - fromTargetToMouse * ratio;
    }
}