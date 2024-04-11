using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform player;
    Vector3 offset;
    Vector3 cameraSpeed;
    Vector3 initialOffset;

    public float minZoomDistance = 20f;
    public float maxZoomDistance = 70f;
    public float maxDistanceFromPlayer = 10f;
    public float mouseMoveSpeed = 0.1f;
    public float minFoV = 20f;
    public float maxFoV = 100f;
    public float zoomFoVSpeed = 10f;

    private Camera cam;

    void Start ( )
    {
        player = GameObject.FindGameObjectWithTag ( "Player" ).transform;
        offset = transform.position - player.position;
        initialOffset = offset;

        cam = GetComponent<Camera> ( );
    }

    void Update ( )
    {
        // Ruch myszk¹ - oœ ZX
        if (Input.GetKey ( KeyCode.LeftControl ))
        {
            float mouseX = Input.GetAxis ( "Mouse X" ) * mouseMoveSpeed;
            float mouseY = Input.GetAxis ( "Mouse Y" ) * mouseMoveSpeed;

            offset += new Vector3 ( mouseX, 0f, mouseY );
        }

        // Zoom kamery za pomoc¹ scrollowania myszy zmieniaj¹c FoV
        float scroll = Input.GetAxis ( "Mouse ScrollWheel" );
        float newFoV = cam.fieldOfView - scroll * zoomFoVSpeed;

        // Ograniczenie zakresu FoV
        newFoV = Mathf.Clamp ( newFoV, minFoV, maxFoV );
        cam.fieldOfView = newFoV;

        // Ograniczenie odleg³oœci kamery od gracza
        if (offset.magnitude < maxDistanceFromPlayer)
        {
            offset = offset.normalized * maxDistanceFromPlayer;
        }

        // Reset pozycji kamery wzglêdem osi XZ
        if (Input.GetKeyDown ( KeyCode.R ))
        {
            ResetCameraPosition ( );
        }

        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp ( transform.position, targetPosition, ref cameraSpeed, 0.3f );
    }

    private void ResetCameraPosition ( )
    {
        // Resetuje pozycjê kamery wzglêdem gracza (Z = -13, X = 0, Y = obecne Y kamery)
        offset = initialOffset;
    }
}
