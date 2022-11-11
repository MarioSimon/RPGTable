using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject UIPanel;

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    public void DragHandler(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;

        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, pointerData.position, canvas.worldCamera, out position);

        UIPanel.transform.position = UIPanel.transform.TransformPoint(position);
    }
}
