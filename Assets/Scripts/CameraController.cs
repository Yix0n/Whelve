using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform player;
    Vector3 offset;
    Vector3 cameraSpeed;

    public float zoomSpeed = 10f;
    public float minZoomDistance = 20f;
    public float maxZoomDistance = 70f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - player.position;
    }

    void Update()
    {
        // Przybli¿anie i oddalanie kamery poprzez scrollowanie
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        offset += transform.forward * scroll * zoomSpeed;
        offset = Vector3.ClampMagnitude(offset, maxZoomDistance);
        offset = Vector3.ClampMagnitude(offset, minZoomDistance);

        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref cameraSpeed, 0.3f);
    }
}