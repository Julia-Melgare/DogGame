using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetObject;
    public float smoothness;
    private Vector3 _initalOffset;
    private bool _isFollowingDog;
    private int _screenBoundary = 20;
    private Plane _plane = new Plane(Vector3.up, Vector3.zero);


    void Start()
    {
        _initalOffset = transform.position - targetObject.position;
        _isFollowingDog = true;
    }

    void Update()
    {
        if (_isFollowingDog)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetObject.transform.position - transform.position), smoothness * Time.deltaTime);
            var cameraPosition = targetObject.position + _initalOffset;
            transform.position = Vector3.Lerp(transform.position, cameraPosition, smoothness * Time.deltaTime);
        }
        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x > Screen.width - _screenBoundary || mousePos.x < _screenBoundary || mousePos.y > Screen.height - _screenBoundary || mousePos.y < _screenBoundary)
        {
            _isFollowingDog = false;
            var ray = Camera.main.ScreenPointToRay(mousePos);
            float enter = 0f;
            if (_plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 revealPos = (hitPoint + targetObject.position) / 2f;
                revealPos = (revealPos + targetObject.position) / 2f;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(revealPos - transform.position), smoothness * Time.deltaTime);
                var cameraPosition = revealPos + _initalOffset;
                transform.position = Vector3.Lerp(transform.position, cameraPosition, smoothness * Time.deltaTime);
            }
        }
        else _isFollowingDog = true;
    }

}
