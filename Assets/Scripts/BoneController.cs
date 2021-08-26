using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneController : MonoBehaviour
{
    private Plane plane = new Plane(Vector3.up, Vector3.zero);
    private Rigidbody rigidbody;
    private bool isGrabbed;
    public float cursorSpeed;
    public float rollSpeed;
    public float fallSpeedBoost;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        isGrabbed = true;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0f;
        if (plane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            transform.position = new Vector3(hitPoint.x, hitPoint.y + 5f, hitPoint.z);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isGrabbed)
            {
                isGrabbed = false;
                rigidbody.useGravity = true;
                rigidbody.freezeRotation = false;
                rigidbody.constraints = RigidbodyConstraints.None;
                rigidbody.velocity = Vector3.down * fallSpeedBoost;
            }
            else
            {
                Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(inputRay, out hit))
                {
                    if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Bone")
                    {
                        isGrabbed = true;
                        rigidbody.useGravity = false;                        
                        transform.rotation = Quaternion.Euler(90, 0, 0);
                        rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                        rigidbody.freezeRotation = true;
                        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        float enter = 0f;
                        if (plane.Raycast(ray, out enter))
                        {
                            Vector3 hitPoint = ray.GetPoint(enter);
                            transform.position = new Vector3(hitPoint.x, hitPoint.y + 5f, hitPoint.z);
                        }
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
                Vector3 direction = hitPoint - transform.position;
                direction = direction.normalized;
                rigidbody.velocity = cursorSpeed * direction;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(!isGrabbed)
        {
            if(LayerMask.LayerToName(collision.gameObject.layer) == "Obstacles")
            {
                Vector3 boneDirection = CalculateBoneDirection(collision.gameObject);
                rigidbody.velocity = rollSpeed * boneDirection;
            }
            else
            {
                rigidbody.velocity = Vector3.down * fallSpeedBoost;
            }
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!isGrabbed && LayerMask.LayerToName(collision.gameObject.layer) == "Obstacles")
            rigidbody.velocity = Vector3.down * fallSpeedBoost;
    }

    Vector3 CalculateBoneDirection(GameObject collidingObject)
    {
        float xsize = collidingObject.GetComponent<Collider>().bounds.size.x;
        float zsize = collidingObject.GetComponent<Collider>().bounds.size.z;
        if(xsize < zsize)
        {
            return Vector3.left;
        }
        else
        {
            return Vector3.back;
        }
    }
}