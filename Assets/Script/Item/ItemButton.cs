using UnityEngine;
using UnityEngine.UI;

namespace DrawColor
{
    public class ItemButton : MonoBehaviour
    {
        [SerializeField] private Item itemType;
        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            ItemManager.Instance.UseItem(itemType);
        }
    }
}

