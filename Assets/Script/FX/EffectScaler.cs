using UnityEngine;

namespace DrawColor
{
    public class EffectScaler : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera; // Camera cần theo dõi
        [SerializeField] private float baseCameraSize = 5f; // Kích thước gốc của Camera (tham chiếu)
        [SerializeField] private float scaleMultiplier = 1f; // Hệ số phóng to/thu nhỏ hiệu ứng

        private void Start()
        {
            if (targetCamera == null)
            {
                targetCamera = Camera.main; // Mặc định lấy Camera chính nếu không chỉ định
            }

            UpdateScale();
        }

        private void LateUpdate()
        {
            UpdateScale();
        }

        private void UpdateScale()
        {
            if (targetCamera == null) return;

            // Tính toán hệ số dựa trên kích thước hiện tại của Camera so với kích thước gốc
            float currentScale = (targetCamera.orthographicSize / baseCameraSize) * scaleMultiplier;

            // Áp dụng hệ số cho GameObject
            transform.localScale = Vector3.one * currentScale;
        }
    }
}

