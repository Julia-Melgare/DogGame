using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneController : MonoBehaviour
{
    private Plane plane = new Plane(Vector3.up, Vector3.zero);
    private Rigidbody rigidBody;
    private bool isGrabbed;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;
        isGrabbed = true;
    }

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0f;

        if (Input.GetMouseButtonDown(0))
        {
            Ray inputRay = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                if (LayerMask.LayerToName(hit.collider.gameObject.layer) != "Obstacles")
                {
                    isGrabbed = false;
                    rigidBody.isKinematic = false;
                }
                else
                {
                    transform.position = RepositionBone(hit);
                    //TODO: check for invalid value before dropping bone
                    isGrabbed = false;
                    rigidBody.isKinematic = false;
                }

            }
        }
        if (isGrabbed)
        {
            if (plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                transform.position = new Vector3(hitPoint.x, hitPoint.y + 5f, hitPoint.z);
            }
        }
    }

    bool isValidPosition(Vector3 position)
    {
        Ray inputRay = new Ray(position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Obstacles")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    Vector3 RepositionBone(RaycastHit hit)
    {
        Vector3 currentPos = transform.position;
        BoxCollider collidingObject = (BoxCollider)hit.collider;
        float xsize = collidingObject.size.x * hit.collider.gameObject.transform.localScale.x;
        float zsize = collidingObject.size.z * hit.collider.gameObject.transform.localScale.z;
        Debug.Log(xsize);
        Debug.Log(zsize);
        Vector3[] directions = { new Vector3(-xsize, 0, 0), new Vector3(0, 0, -zsize), new Vector3(xsize, 0, 0), new Vector3(0, 0, zsize) };
        while (directions[0].x <= xsize * 2f)
        {
            for (int j = 0; j < directions.Length; j++)
            {
                Vector3 newPos = currentPos + directions[j];
                if (isValidPosition(newPos))
                {
                    return newPos;
                }
                else
                {
                    switch (j)
                    {
                        case 0:
                            directions[j] += new Vector3(-2, 0, 0);
                            break;
                        case 1:
                            directions[j] += new Vector3(0, 0, -2);
                            break;
                        case 2:
                            directions[j] += new Vector3(2, 0, 0);
                            break;
                        case 3:
                            directions[j] += new Vector3(0, 0, 2);
                            break;
                    }
                }

            }
        }          
        return currentPos; //TODO: return invalid value
    }



}