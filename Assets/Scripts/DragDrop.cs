using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Canvas canvas;
    [SerializeField] RectTransform panel;
    [SerializeField] Transform building;
    [SerializeField] LayerMask layerMask;

    RectTransform rect;
    CanvasGroup canvasGroup;
    PlaceableObject po;
    public static event EventHandler outsideEvent;

    bool spawned = false;
    Ray ray;
    RaycastHit hit;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        building.gameObject.SetActive(false);
        po = building.GetComponent<PlaceableObject>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        Vector3 pos = panel.InverseTransformPoint(eventData.position);
        if (!panel.rect.Contains(pos) && !spawned)
        {
            outsideEvent.Invoke(null, EventArgs.Empty);
            ray = Camera.main.ScreenPointToRay(eventData.position);
            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                building.gameObject.SetActive(true);
            }
            spawned = true;
        }
        else if (panel.rect.Contains(pos) && spawned)
        {
            building.gameObject.SetActive(false);
            spawned = false;
        }
        else if (spawned)
        {
            ray = Camera.main.ScreenPointToRay(eventData.position);
            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                po.FollowPosition(hit.point, Quaternion.identity);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 pos = panel.InverseTransformPoint(eventData.position);
        if (!panel.rect.Contains(pos))
        {
            po.PlaceBuilding();
            outsideEvent.Invoke(null, EventArgs.Empty);
        }
        building.gameObject.SetActive(false);

        canvasGroup.blocksRaycasts = true;
        rect.anchoredPosition = Vector3.zero;
    }
}