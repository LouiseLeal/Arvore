using UnityEngine;
using UnityEngine.UI;

namespace Snake
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] protected SpriteData spriteData;

        [SerializeField] protected Image image;
        [SerializeField] protected RectTransform rectTransform;

        public int x = 0, y = 0;

        public void SetTileSize(Vector2 tileSize)
        {
            rectTransform.sizeDelta = tileSize;
        }
    }
}


