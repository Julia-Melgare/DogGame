using UnityEngine;
public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minCamSize = 3f;
    [SerializeField] private float maxCamSize = 8f;

    private void Update()
    {
        float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        float newZoomLevel = cam.orthographicSize - mouseScrollWheel;
        cam.orthographicSize = Mathf.Clamp(newZoomLevel, minCamSize, maxCamSize);
    }
}

//based on code from: https://answers.unity.com/questions/384753/ortho-camera-zoom-to-mouse-point.html