using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    [SerializeField] public GameObject mainPrefab;
    [SerializeField] float length; //size in x
    [SerializeField] float breadth; //size in y
    [SerializeField] Material material;
    [SerializeField] bool player = false;

    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void FollowPosition(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        if (IsPlaceable())
        {
            material.color = Color.green;
        }
        else
            material.color = Color.red;
    }

    public void PlaceBuilding()
    {
        if (IsPlaceable())
        {
            if (!player)
            {
                Instantiate(mainPrefab, transform.position, Quaternion.identity).GetComponent<EditBuilding>().ghost = gameObject;
                GameManager.gm.PlayPlaceEffect(transform.position);
            }
            else
                mainPrefab.GetComponent<Rigidbody>().MovePosition(transform.position);
        }
        else
            return;
    }

    public bool IsPlaceable()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(length / 2, 1, breadth / 2), Quaternion.identity);

        if (transform.position.x + length / 2 > GameManager.gm.mapBounds.x || transform.position.x - length / 2 < -GameManager.gm.mapBounds.x
            || transform.position.z + breadth / 2 > GameManager.gm.mapBounds.y || transform.position.z - breadth / 2 < -GameManager.gm.mapBounds.y)
            return false;

        if (colliders.Length > 1)
        {
            return false;
        }
        else
            return true;
    }
}
