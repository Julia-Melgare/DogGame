using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetObject;
    public float smoothness;
    private Vector3 initalOffset;
    private Vector3 cameraPosition;

    void Start()
    {
        initalOffset = transform.position - targetObject.position;
    }

    void Update()
    {
        //transform.LookAt(targetObject);
        cameraPosition = targetObject.position + initalOffset;
        transform.position = Vector3.Lerp(transform.position, cameraPosition, smoothness*Time.deltaTime);
    }
}
