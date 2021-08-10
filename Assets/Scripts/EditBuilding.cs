using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditBuilding : MonoBehaviour
{
    [SerializeField] GameObject buttons;
    public GameObject ghost;
    public bool selected = false, canMove = false;

    MeshRenderer meshRenderer;
    Collider objectCollider;
    PlaceableObject po;
    Vector3 lastPos;
    Vector3 offset;
    RectTransform rectTransform;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        objectCollider = GetComponent<Collider>();
        po = ghost.GetComponent<PlaceableObject>();
        rectTransform = buttons.GetComponent<RectTransform>();

        if (selected)
            buttons.SetActive(true);
        else
            buttons.SetActive(false);

        offset = new Vector3(0, -100, 0);
        rectTransform.position = Camera.main.WorldToScreenPoint(transform.position) + offset;
    }

    public void Follow(Vector3 pos)
    {
        transform.position = pos;
        po.FollowPosition(pos, transform.rotation);
        if (po.IsPlaceable())
        {
            lastPos = transform.position;
        }
        rectTransform.position = Camera.main.WorldToScreenPoint(transform.position) + offset;
    }

    public void EnableMove()
    {
        canMove = true;
        meshRenderer.enabled = false;
        objectCollider.enabled = false;
        po.FollowPosition(transform.position, transform.rotation);
        ghost.SetActive(true);
    }

    public void DisableMove()
    {
        canMove = false;
        meshRenderer.enabled = true;
        objectCollider.enabled = true;
        ghost.SetActive(false);
        Follow(lastPos);
    }

    public void Select()
    {
        selected = true;
        buttons.SetActive(true);
        GameManager.gm.HideMenu();
    }

    public void DeSelect()
    {
        //DisableMove();
        if (this)
        {
        selected = false;
        buttons.SetActive(false);
        }
    }

    public void DeleteBuilding()
    {
        Destroy(gameObject);
    }

    public void RotateRight()
    {
        transform.forward = transform.right;
    }
}