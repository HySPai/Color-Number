using TMPro;
using UnityEngine;
using System.Collections;

namespace DrawColor
{
    public class ColorSprite : SpriteManager
    {
        private SpriteRenderer spriteRenderer;
        private SpriteMask mask;
        private PolygonCollider2D polygonCollider;
        private bool isLocker = false;
        private static Color? activeColor = null;
        private static ColorButton activeButton = null;

        [SerializeField] private TextMeshPro ID;
        [SerializeField] private VFX spriteVFX = VFX.normal;
        [SerializeField] private float duration = 0.3f;

        private string spriteID;
        private bool isColorTransitionComplete = false;

        [SerializeField] private bool freezeSprite;
        [SerializeField, ConditionalField("freezeSprite")] private int freezeBreak = 3;
        private int currentBreaks = 0;

        protected override void Start()
        {
            base.Start();

            spriteRenderer = GetComponent<SpriteRenderer>();
            mask = GetComponent<SpriteMask>();
            ID = GetComponentInChildren<TextMeshPro>();
            polygonCollider = GetComponent<PolygonCollider2D>();

            if (spriteRenderer == null)
            {
                Debug.Log("SpriteRenderer is missing on this object.");
            }

            if (mask == null)
            {
                Debug.Log("SpriteMask is missing on this object.");
            }
            else
            {
                mask.sprite = spriteRenderer.sprite;
                mask.enabled = false;
            }

            if (polygonCollider == null)
            {
                Debug.Log("polygonCollider is missing on this object.");
            }
            else
            {
                polygonCollider.enabled = false;
            }

            freezeSprite = spriteVFX == VFX.freeze;
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 0; // Cập nhật giá trị Alpha
                spriteRenderer.color = color;
            }
        }

        void OnValidate()
        {
            freezeSprite = spriteVFX == VFX.freeze;
        }
        void OnMouseDown()
        {
            if (isColorTransitionComplete)
            {
                Debug.Log($"Sprite {gameObject.name} has already been fully colored.");
                return;
            }

            if (isLocker)
                return;
            
            if (activeColor.HasValue && activeButton != null)
            {
                if (freezeSprite)
                {
                    currentBreaks++;
                    Debug.Log($"Breaking ice: {currentBreaks}/{freezeBreak}");

                    if (currentBreaks >= freezeBreak)
                    {
                        freezeSprite = false;
                        Debug.Log($"Ice broken on {gameObject.name}!");
                    }
                    else
                    {
                        return;
                    }
                }

                if (activeButton.targetSprites.Contains(gameObject))
                {
                    ParticleSystem effectPrefab = spriteVFX == VFX.freeze
                        ? activeButton.vfxColor.colorFreeze
                        : activeButton.vfxColor.colorNormal;

                    if (effectPrefab != null)
                    {
                        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        mousePosition.z = 0f;

                        ParticleSystem effectInstance = Instantiate(effectPrefab, mousePosition, Quaternion.identity);

                        if (spriteVFX == VFX.normal)
                        {
                            var mainModule = effectInstance.main;
                            mainModule.startColor = activeButton.ButtonColor;
                        }

                        Destroy(effectInstance.gameObject, effectInstance.main.duration);

                        StartCoroutine(SpawnMovingEffectToSlider(mousePosition));
                    }

                    StartCoroutine(ChangeColorFromPosition(activeColor.Value, effectPrefab));
                }
            }
        }

        #region Color
        public static void SetActiveColor(Color color, ColorButton button)
        {
            activeColor = color;
            activeButton = button;
            Debug.Log("Active color set to: " + color);
        }

        public void SetAlpha(float alpha)
        {
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = alpha; // Cập nhật giá trị Alpha
                spriteRenderer.color = color;
            }
        }

        public bool IsFullyColored(Color colorToCheck)
        {
            return isColorTransitionComplete && spriteRenderer.color == colorToCheck;
        }

        public IEnumerator ChangeColorFromPosition(Color targetColor, ParticleSystem effect, float duration = 0.3f)
        {
            isLocker = true;
            float time = 0f;
            Color initialColor = spriteRenderer.color;
            isColorTransitionComplete = false;
            SetMaskActive(false);
            SetPolygonColliderActive(false);
            while (time < duration)
            {
                spriteRenderer.color = Color.Lerp(initialColor, targetColor, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            spriteRenderer.color = targetColor;
            isColorTransitionComplete = true;

            ClearIDText();
            ButtonManager.Instance.IncrementColoredSprites();

            if (activeButton != null)
            {
                activeButton.CheckAndDestroyButton();
            }
        }
        #endregion

        #region Set Active
        public void SetMaskActive(bool isActive)
        {
            if (mask != null)
            {
                mask.enabled = isActive;
            }
        }
        public void SetPolygonColliderActive(bool isActive)
        {
            if (polygonCollider != null)
            {
                polygonCollider.enabled = isActive;
            }
        }
        #endregion

        #region VFX
        public VFX GetSpriteVFX()
        {
            return spriteVFX;
        }
        public IEnumerator SpawnMovingEffectToSlider(Vector3 start)
        {
            if (ButtonManager.Instance.sliderFill == null)
            {
                Debug.LogError("Slider Fill is not assigned in ButtonManager.");
                yield break;
            }

            // Lấy vị trí của sliderFill
            Vector3 end = ButtonManager.Instance.sliderFill.position;
            ParticleSystem movingEffect = Instantiate(activeButton.vfxColor.colorMoveEffect, start, Quaternion.identity);
            var mainModule = movingEffect.main;

            mainModule.startColor = activeButton.ButtonColor;

            float time = 0f;

            while (time < duration)
            {
                // Di chuyển hiệu ứng từ start đến vị trí sliderFill
                movingEffect.transform.position = Vector3.Lerp(start, end, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            // Đặt vị trí cuối cùng tại sliderFill
            movingEffect.transform.position = end;

            // Hủy hiệu ứng sau khi hoàn thành
            Destroy(movingEffect.gameObject);
        }

        /*
private IEnumerator SpawnMovingEffect(Vector3 start, Vector3 end)
{
    ParticleSystem movingEffect = Instantiate(activeButton.vfxColor.colorMoveEffect, start, Quaternion.identity);
    var mainModule = movingEffect.main;

    mainModule.startColor = activeButton.ButtonColor;

    float time = 0f;

    while (time < duration)
    {
        // Di chuyển hiệu ứng từ start đến end
        movingEffect.transform.position = Vector3.Lerp(start, end, time / duration);
        time += Time.deltaTime;
        yield return null;
    }

    // Đặt vị trí cuối cùng tại end
    movingEffect.transform.position = end;

    // Hủy hiệu ứng sau khi hoàn thành
    Destroy(movingEffect.gameObject);
}
*/
        #endregion

        #region ID
        public string GetSpriteID()
        {
            return spriteID;
        }

        public void UpdateIDText(string newID)
        {
            if (ID != null)
            {
                ID.text = newID;
            }
        }

        private void ClearIDText()
        {
            if (ID != null)
            {
                Destroy(ID.gameObject);
                Debug.Log("ID TextMeshProUGUI has been destroyed.");
            }
        }

        public void SetIDText(TextMeshPro text)
        {
            ID = text;
        }
        #endregion
    }
}
