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
        if (Input.GetMouseButtonDown(0))
        {
            if (isGrabbed)
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
                        Vector3 newPos = RepositionBone(hit);
                        if (!float.IsNaN(newPos.x))
                        {
                            transform.position = newPos;
                            isGrabbed = false;
                            rigidBody.isKinematic = false;
                        }
                    }

                }
            }
            else
            {
                Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(inputRay, out hit))
                {
                    if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Bone")
                    {
                        rigidBody.isKinematic = true;
                        isGrabbed = true;
                        transform.rotation = Quaternion.Euler(90, 0, 0);
                    }
                }
            }
            
        }
        if (isGrabbed)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter = 0f;
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
        Vector3[] directions = { new Vector3(-xsize, 0, 0), new Vector3(0, 0, -zsize), new Vector3(xsize, 0, 0), new Vector3(0, 0, zsize) };
        List<Vector3> sortedDirections = new List<Vector3>(directions);
        sortedDirections.Sort((a,b) => a.magnitude.CompareTo(b.magnitude));
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
                            sortedDirections[j] += new Vector3(-2, 0, 0);
                            break;
                        case 1:
                            sortedDirections[j] += new Vector3(0, 0, -2);
                            break;
                        case 2:
                            sortedDirections[j] += new Vector3(2, 0, 0);
                            break;
                        case 3:
                            sortedDirections[j] += new Vector3(0, 0, 2);
                            break;
                    }
                }

            }
        }          
        return new Vector3(float.NaN, float.NaN, float.NaN);
    }



}