using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Camera secondaryCamera; // Tham chiếu đến camera có Clear Flags là Depth Only
    [SerializeField] private float zoomSpeed = 0.1f; // Tốc độ zoom
    [SerializeField] private float panSpeed = 0.5f; // Tốc độ kéo camera
    [SerializeField] private float minZoom = 0.5f; // Giới hạn zoom nhỏ nhất
    [SerializeField] private float maxZoom = 5f; // Giới hạn zoom lớn nhất

    [SerializeField] private Vector2 panLimitMin;
    [SerializeField] private Vector2 panLimitMax;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        if (secondaryCamera == null)
        {
            Debug.LogError("Secondary Camera is not assigned!");
        }
    }

    void Update()
    {
        HandleZoom();
        HandlePan();
        SyncSecondaryCameraFOV();
    }

    private void HandleZoom()
    {
        // Xử lý zoom bằng 2 ngón tay trên thiết bị di động
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Tính khoảng cách giữa hai ngón tay
            float prevDistance = (touch1.position - touch1.deltaPosition - (touch2.position - touch2.deltaPosition)).magnitude;
            float currentDistance = (touch1.position - touch2.position).magnitude;

            // Tính độ chênh lệch để zoom
            float deltaDistance = prevDistance - currentDistance;
            float zoomAmount = deltaDistance * zoomSpeed * Time.deltaTime;

            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + zoomAmount, minZoom, maxZoom);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float zoomAmount = -scroll * zoomSpeed * 100f;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + zoomAmount, minZoom, maxZoom);
        }
    }

    private void HandlePan()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Touch touch = Input.GetTouch(0);

            // Điều chỉnh tốc độ pan dựa trên mức zoom
            float adjustedPanSpeed = panSpeed * cam.orthographicSize;

            // Lấy giá trị di chuyển
            Vector3 deltaPosition = new Vector3(-touch.deltaPosition.x, -touch.deltaPosition.y, 0) * adjustedPanSpeed * Time.deltaTime;

            // Di chuyển camera
            transform.position += deltaPosition;

            // Giới hạn camera trong vùng quy định
            float clampedX = Mathf.Clamp(transform.position.x, panLimitMin.x, panLimitMax.x);
            float clampedY = Mathf.Clamp(transform.position.y, panLimitMin.y, panLimitMax.y);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }

    private void SyncSecondaryCameraFOV()
    {
        if (secondaryCamera != null)
        {
            secondaryCamera.fieldOfView = cam.orthographicSize; // Đồng bộ hóa FOV với camera chính
        }
    }
}
