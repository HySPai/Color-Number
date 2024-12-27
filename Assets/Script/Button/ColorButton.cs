using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System.Collections;

namespace DrawColor
{
    public class ColorButton : MonoBehaviour
    {
        public List<GameObject> targetSprites;
        public bool IsSelected { get; private set; }

        [SerializeField] private Color buttonColor;
        public Color ButtonColor => buttonColor;
        public Color SpriteColor => spriteColor;
        [SerializeField] private Color spriteColor;
        [SerializeField] private string idText;
 
        [Header("VFX")]
        public ButtonColorVFX vfxColor; 
        [SerializeField] private GameObject effectObject;
        [SerializeField] private float effectDuration = 0.5f; // Thời gian chạy hiệu ứng

        public Slider sliderColor;
        public SpriteManager spriteManager;
        private Animator buttonAnimator;
        private Button button;
        private static ColorButton activeButton = null;

        private void Start()
        {
            button = GetComponentInChildren<Button>();
            buttonAnimator = GetComponentInChildren<Animator>();
            sliderColor = GetComponentInChildren<Slider>();

            button.onClick.AddListener(OnButtonClick);

            ButtonManager.Instance.RegisterButton(this);

            if (sliderColor != null)
            {
                sliderColor.maxValue = targetSprites.Count;
                sliderColor.value = 0;
            }

            UpdateButtonColors();
            UpdateIDText();
            SetAnimationState("Idle");
        }


        private void Update()
        {
            UpdateTargetSpritesText();
        }

        private void OnDestroy()
        {
            ButtonManager.Instance.UnregisterButton(this);
        }

        public void OnButtonClick()
        {
            if (spriteManager == null)
            {
                return;
            }

            // Tắt mask của tất cả các ColorSprite trong các button khác
            if (activeButton != null && activeButton != this)
            {
                activeButton.SetAnimationState("Idle");

                foreach (var spriteObj in activeButton.targetSprites)
                {
                    if (spriteObj != null)
                    {
                        ColorSprite colorSprite = spriteObj.GetComponent<ColorSprite>();

                        if (colorSprite != null && !colorSprite.IsFullyColored(activeButton.spriteColor))
                        {
                            colorSprite.SetMaskActive(false); // Tắt mask
                            colorSprite.SetPolygonColliderActive(false);
                        }
                    }
                }
            }

            // Cập nhật activeButton
            activeButton = this;
            SetAnimationState("OnClick");

            // Kích hoạt mask cho các ColorSprite thuộc button hiện tại
            foreach (var spriteObj in targetSprites)
            {
                if (spriteObj != null)
                {
                    ColorSprite colorSprite = spriteObj.GetComponent<ColorSprite>();

                    if (colorSprite != null && !colorSprite.IsFullyColored(spriteColor))
                    {
                        colorSprite.SetAlpha(0); // Đặt alpha làm nổi bật
                        colorSprite.SetMaskActive(true); // Bật mask
                        colorSprite.SetPolygonColliderActive(true);
                    }
                }
            }

            // Kích hoạt màu sắc hiện tại
            ColorSprite.SetActiveColor(spriteColor, this);
        }

        public void ActivateSprite(bool active)
        {
            foreach (var sprite in targetSprites)
            {
                var colorSprite = sprite.GetComponent<ColorSprite>();
                if (colorSprite != null)
                {
                    colorSprite.SetMaskActive(active);
                    colorSprite.SetPolygonColliderActive(active);
                }
            }
        }

        public void UpdateSlider()
        {
            if (sliderColor != null)
            {
                sliderColor.value++; // Tăng giá trị slider lên 1

                // Kiểm tra nếu slider đã đầy
                if (sliderColor.value >= sliderColor.maxValue)
                {
                    Debug.Log("All sprites colored for this button! Destroying button.");

                    // Unregister và phá hủy button
                    ButtonManager.Instance.UnregisterButton(this);
                    Destroy(gameObject);

                    // Kiểm tra nếu không còn nút nào hoạt động
                    CheckAllButtonsDestroyed();
                }
            }
        }

        public void CheckAndDestroyButton()
        {
            // Đếm số sprite đã tô đầy đủ
            int coloredCount = targetSprites.Count(spriteObj =>
            {
                if (spriteObj != null)
                {
                    ColorSprite colorSprite = spriteObj.GetComponent<ColorSprite>();
                    if (colorSprite != null)
                    {
                        return colorSprite.IsFullyColored(spriteColor);
                    }
                }
                return false;
            });

            // Cập nhật slider
            if (sliderColor != null)
            {
                sliderColor.value = coloredCount;
            }

            // Kiểm tra nếu tất cả đã được tô
            if (coloredCount == targetSprites.Count)
            {
                Debug.Log("All sprites colored for this button! Creating destruction effect.");

                // Tạo hiệu ứng phá hủy
                if (effectObject != null)
                {
                    GameObject destructionEffect = Instantiate(effectObject, transform.position, Quaternion.identity);
                    destructionEffect.SetActive(true);

                    // Đảm bảo hiệu ứng đồng bộ với buttonColor
                    ParticleSystem particleSystem = destructionEffect.GetComponent<ParticleSystem>();
                    if (particleSystem != null)
                    {
                        var mainModule = particleSystem.main;
                        mainModule.startColor = buttonColor;
                    }
                    Destroy(destructionEffect, effectDuration);
                }

                // Phá hủy nút
                DestroyButton();
            }
        }
        private void DestroyButton()
        {
            ButtonManager.Instance.UnregisterButton(this); // Hủy đăng ký
            Destroy(gameObject); // Phá hủy button
            CheckAllButtonsDestroyed(); // Kiểm tra nếu tất cả các nút đã bị phá hủy
        }

        private void CheckAllButtonsDestroyed()
        {
            if (ButtonManager.Instance.GetAllButtons().Count == 0) // Kiểm tra danh sách các nút
            {
                Debug.Log("All buttons destroyed! Switching to finish menu.");
                if (UI_Manager.instance != null)
                {
                    UI_Manager.instance.SwitchMenuTo(UI_Manager.instance.finishMenu);
                }
            }
        }

        private void UpdateButtonColors()
        {
            #region Color Button
            ColorBlock colorBlock = button.colors;

            colorBlock.normalColor = buttonColor;
            colorBlock.selectedColor = buttonColor;

            colorBlock.highlightedColor = buttonColor * 1.2f; // Màu sáng hơn 20%
            colorBlock.pressedColor = buttonColor * 1f;      // Màu tối hơn 20%
            colorBlock.disabledColor = new Color(buttonColor.r, buttonColor.g, buttonColor.b, 1f); // Màu trong suốt

            button.colors = colorBlock;
            #endregion

            #region Particle System Color
            if (effectObject != null)
            {
                ParticleSystem particleSystem = effectObject.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    var mainModule = particleSystem.main;
                    mainModule.startColor = buttonColor; // Đặt màu cho ParticleSystem
                }
            }
            #endregion
        }


        private void UpdateTargetSpritesText()
        {
            foreach (var spriteObj in targetSprites)
            {
                if (spriteObj != null)
                {
                    ColorSprite colorSprite = spriteObj.GetComponent<ColorSprite>();
                    if (colorSprite != null)
                    {
                        colorSprite.UpdateIDText(idText);
                    }
                }
            }
        }

        private void SetAnimationState(string state)
        {
            if (buttonAnimator != null)
            {
                buttonAnimator.Play(state);
            }
        }

        public void SetButtonID(string id)
        {
            idText = id; // Cập nhật giá trị idText
            UpdateIDText(); // Hiển thị giá trị trên TextMeshPro
        }

        private void UpdateIDText()
        {
            TextMeshProUGUI textMesh = GetComponentInChildren<TextMeshProUGUI>();
            if (textMesh != null)
            {
                textMesh.text = idText;
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI not found for ColorButton: " + gameObject.name);
            }
        }
    }
}
