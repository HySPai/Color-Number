using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DrawColor
{
    public class SpriteManager : MonoBehaviour
    {
        [Header("ID Text Prefab")]
        public TextMeshPro idTextPrefabs;

        private List<ColorSprite> colorSprites = new List<ColorSprite>();

        protected virtual void Awake()
        {
            colorSprites = new List<ColorSprite>(GetComponentsInChildren<ColorSprite>());

            if (idTextPrefabs == null)
            {
                Debug.Log("ID Text Prefab is not assigned in SpriteManager!");
            }
        }

        protected virtual void Start()
        {
            if (idTextPrefabs == null)
            {
                return;
            }

            foreach (var sprite in colorSprites)
            {
                CreateIDTextForSprite(sprite);
            }
        }

        private void CreateIDTextForSprite(ColorSprite sprite)
        {
            if (sprite == null || idTextPrefabs == null)
            {
                return;
            }

            TextMeshPro newText = Instantiate(idTextPrefabs, sprite.transform);

            newText.transform.localPosition = Vector3.zero;

            sprite.SetIDText(newText);

            Debug.Log($"Created ID Text for {sprite.name}");
        }
    }
}
