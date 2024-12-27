using DrawColor;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    private void Awake()
    {
        // Đảm bảo Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UseItem(Item item)
    {
        // Kiểm tra màu hiện tại
        ColorButton selectedButton = ButtonManager.Instance.GetAllButtons().Find(button => button.IsSelected);
        if (selectedButton == null)
        {
            Debug.Log("Không có màu nào đang được chọn!");
            return;
        }

        switch (item)
        {
            case Item.Small:
                ApplySmallEffect(selectedButton);
                break;

            case Item.Super:
                ApplySuperEffect(selectedButton);
                break;
        }
    }

    private void ApplySmallEffect(ColorButton selectedButton)
    {
        List<GameObject> targetGameObjects = selectedButton.targetSprites; // Danh sách GameObject
        int count = 0;

        foreach (var obj in targetGameObjects)
        {
            SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
            if (sprite != null && !sprite.color.Equals(selectedButton.SpriteColor)) // Chỉ tô nếu chưa được tô
            {
                sprite.color = selectedButton.SpriteColor;
                ButtonManager.Instance.IncrementColoredSprites();
                count++;

                if (count >= 3) // Tô tối đa 3 sprite
                    break;

                selectedButton.CheckAndDestroyButton();
            }
        }
    }

    private void ApplySuperEffect(ColorButton selectedButton)
    {
        List<GameObject> targetGameObjects = selectedButton.targetSprites; // Danh sách GameObject

        foreach (var obj in targetGameObjects)
        {
            SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
            if (sprite != null && !sprite.color.Equals(selectedButton.SpriteColor)) // Chỉ tô nếu chưa được tô
            {
                sprite.color = selectedButton.SpriteColor;
                ButtonManager.Instance.IncrementColoredSprites();
                selectedButton.CheckAndDestroyButton();
            }
        }
    }

}
