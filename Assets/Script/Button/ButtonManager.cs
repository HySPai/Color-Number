using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrawColor
{
    public class ButtonManager : MonoBehaviour
    {
        public static ButtonManager Instance;

        [SerializeField] private List<ColorButton> colorButtons = new List<ColorButton>();

        [SerializeField] private Slider sliderColor;
        [SerializeField] private TextMeshProUGUI textSlider;
        public RectTransform sliderFill;

        private int totalSprites = 0;
        private int coloredSprites = 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            AssignSequentialIDs();
            CalculateTotalSprites();
            UpdateSlider();
            StartCoroutine(SelectFirstButtonWithDelay());
        }

        private IEnumerator SelectFirstButtonWithDelay()
        {
            yield return null;
            SelectFirstButton();
        }

        public void RegisterButton(ColorButton button)
        {
            if (!colorButtons.Contains(button))
            {
                colorButtons.Add(button);
            }
        }

        public void UnregisterButton(ColorButton button)
        {
            if (colorButtons.Contains(button))
            {
                int index = colorButtons.IndexOf(button);
                colorButtons.Remove(button);

                // Chọn nút tiếp theo (nếu có) sau khi xóa
                if (colorButtons.Count > 0)
                {
                    int nextIndex = Mathf.Clamp(index, 0, colorButtons.Count - 1);
                    colorButtons[nextIndex].OnButtonClick(); // Kích hoạt nút tiếp theo
                }
                else
                {
                    Debug.Log("No more buttons left in the list.");
                }
            }
        }


        public List<ColorButton> GetAllButtons()
        {
            return colorButtons;
        }

        private void CalculateTotalSprites()
        {
            totalSprites = 0;
            foreach (var button in colorButtons)
            {
                totalSprites += button.targetSprites.Count;
            }
        }

        public void IncrementColoredSprites()
        {
            coloredSprites++;
            UpdateSlider();
        }

        private void UpdateSlider()
        {
            if (sliderColor != null)
            {
                float progress = (float)coloredSprites / totalSprites;
                sliderColor.value = progress;

                if (textSlider != null)
                {
                    textSlider.text = $"{Mathf.RoundToInt(progress * 100)}%";
                }
            }
        }

        public void AssignSequentialIDs()
        {
            for (int i = 0; i < colorButtons.Count; i++)
            {
                colorButtons[i].SetButtonID((i + 1).ToString());
            }
        }

        public void SelectFirstButton()
        {
            if (colorButtons.Count > 0)
            {
                colorButtons[0].OnButtonClick();
            }
        }
    }
}
