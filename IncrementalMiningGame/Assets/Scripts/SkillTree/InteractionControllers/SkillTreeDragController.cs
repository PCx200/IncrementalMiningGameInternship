using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTreeDragController : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private RectTransform draggableTransform;
    private Vector2 dragOffset;

    [Header("Dragable region")]
    [SerializeField]
    private float minX;

    [SerializeField] 
    private float maxX;

    [SerializeField] 
    private float minY;

    [SerializeField]
    private float maxY;

    private void Awake()
    {
        draggableTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransform parent = draggableTransform.parent as RectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parent,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        dragOffset = (Vector3)localPoint - draggableTransform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform parent = draggableTransform.parent as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parent,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        ))
        {
            Vector3 newPosition = localPoint - dragOffset;

            float scaleX = draggableTransform.localScale.x;
            float scaleY = draggableTransform.localScale.y;

            // Zoom‑aware clamping
            float scaledMinX = minX * scaleX;
            float scaledMaxX = maxX * scaleX;
            float scaledMinY = minY * scaleY;
            float scaledMaxY = maxY * scaleY;

            newPosition.x = Mathf.Clamp(newPosition.x, scaledMinX, scaledMaxX);
            newPosition.y = Mathf.Clamp(newPosition.y, scaledMinY, scaledMaxY);

            draggableTransform.localPosition = newPosition;
        }
    }

    private void OnDrawGizmos()
    {
        if (draggableTransform == null)
        { 
            draggableTransform = GetComponent<RectTransform>();
        }

        Gizmos.color = Color.green;

        // Draw rectangle in parent local space
        Vector3 downLeft = new Vector3(minX, minY, 0);
        Vector3 downRight = new Vector3(maxX, minY, 0);
        Vector3 upRight = new Vector3(maxX, maxY, 0);
        Vector3 upLeft = new Vector3(minX, maxY, 0);

        // Convert to world space so Scene View shows it correctly
        Transform parent = draggableTransform.parent;

        downLeft = parent.TransformPoint(downLeft);
        downRight = parent.TransformPoint(downRight);
        upRight = parent.TransformPoint(upRight);
        upLeft = parent.TransformPoint(upLeft);

        Gizmos.DrawLine(downLeft, downRight);
        Gizmos.DrawLine(downRight, upRight);
        Gizmos.DrawLine(upRight, upLeft);
        Gizmos.DrawLine(upLeft, downLeft);
    }
}