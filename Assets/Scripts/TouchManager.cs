using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    EditBuilding selectedBuilding;
    EditBuilding buildingHit;
    GameObject go;
    Transform transformHit;

    RaycastHit hit;
    Ray ray;
    bool selected = false, moving = false;

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            moving = selectedBuilding.canMove;
        }

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit))
                {
                    buildingHit = hit.transform.GetComponent<EditBuilding>();
                    if (buildingHit)
                    {
                        transformHit = hit.transform;
                    }
                    else
                        transformHit = null;
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (selected && moving)
                {
                    ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    if (Physics.Raycast(ray, out hit,1000,layerMask))
                    {
                        selectedBuilding.Follow(hit.point);
                    }
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (!selected)
                {
                    ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    if (transformHit && Physics.Raycast(ray, out hit))
                    {
                        if (transformHit == hit.transform)
                        {
                            selectedBuilding = hit.transform.GetComponent<EditBuilding>();
                            selected = true;
                            selectedBuilding.Select();
                        }
                    }
                }
                else if (!moving)
                {
                    ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    if (transformHit && Physics.Raycast(ray, out hit))
                    {
                        if (transformHit == hit.transform)
                        {
                            selectedBuilding.DeSelect();
                            selectedBuilding = hit.transform.GetComponent<EditBuilding>();
                            selected = true;
                            selectedBuilding.Select();
                        }
                    }
                    else
                    {
                        selectedBuilding.DeSelect();
                        selectedBuilding = null;
                        selected = false;
                    }
                }
                else
                {
                }
            }
        }
    }
}
