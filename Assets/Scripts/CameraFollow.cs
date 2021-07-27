using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetObject;
    public float smoothness;
    private Vector3 _initalOffset;
    private bool _isFollowingDog;
    private bool _isShowingAhead;
    private int _screenBoundary = 20;


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
            MoveAhead(mousePos);
            _isFollowingDog = true;
        }

    }

    void MoveAhead(Vector3 pos)
    {
        Vector3 revealPos = transform.position;
        if (pos.x > Screen.width - _screenBoundary)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(revealPos.x + 50f, revealPos.y, revealPos.z), 0.5f * Time.deltaTime);
        }

        if (pos.x < _screenBoundary)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(revealPos.x - 50f, revealPos.y, revealPos.z), 0.5f * Time.deltaTime);
        }

        if (pos.y > Screen.height - _screenBoundary)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(revealPos.x, revealPos.y + 50f, revealPos.z), 0.5f * Time.deltaTime);
        }

        if (pos.y < _screenBoundary)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(revealPos.x, revealPos.y - 50f, revealPos.z), 0.5f * Time.deltaTime);
        }
    }
}
