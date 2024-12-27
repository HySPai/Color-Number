using UnityEngine;
using TMPro;

public class IDVisibilityByFontSize : MonoBehaviour
{
    private Camera cam; // Camera chính
    [SerializeField] private float maxZoom = 5f; // Giá trị zoom tối đa
    [SerializeField] private float minZoomCamera = 2f; // Giá trị zoom tối thiểu

    private TextMeshPro text; // Tham chiếu đến TextMeshProUGUI
    private float fontSize; // Kích thước font của text

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main; // Lấy camera chính nếu chưa được gán
        }

        // Tìm TextMeshProUGUI trong các thành phần con
        text = GetComponentInChildren<TextMeshPro>();

        if (text == null)
        {
            Debug.LogWarning("No TextMeshProUGUI found in children of " + gameObject.name);
            return;
        }

        fontSize = text.fontSize; // Lưu kích thước font để sử dụng
    }

    void Update()
    {
        HandleTextVisibility();
    }

    private void HandleTextVisibility()
    {
        if (text == null) return;

        // Tính phần trăm zoom dựa trên giá trị zoom hiện tại
        float zoomPercent = Mathf.InverseLerp(maxZoom, minZoomCamera, cam.orthographicSize);

        // Quy định logic hiển thị dựa trên kích thước font và mức zoom
        if (fontSize < 0.2f) // Text nhỏ nhất
        {
            text.enabled = zoomPercent > 0.8f; // Hiển thị khi zoom gần
        }
        else if (fontSize >= 0.2f && fontSize < 1f) // Text nhỏ và trung bình
        {
            text.enabled = zoomPercent > 0.5f; // Hiển thị khi zoom trung bình
        }
        else if (fontSize >= 1f) // Text lớn
        {
            text.enabled = zoomPercent > 0.2f; // Hiển thị khi zoom xa hơn
        }
        else
        {
            text.enabled = false; // Ẩn trong các trường hợp khác
        }
    }
}
