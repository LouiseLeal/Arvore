using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Snake
{
    public class Block : MonoBehaviour
    {
        Posiiton position;
        public BlockType type;
        [SerializeField] RectTransform rectTransform;
        [SerializeField] Image image;


        public void SetBlockSize(Vector2 tileSize)
        {
            rectTransform.sizeDelta = tileSize;
        }

        public void SetBlock(BlockType type, Vector2 position)
        {
            if (type == BlockType.INACTIVE)
                image.enabled = false;
            else
                image.enabled = true;


            rectTransform.anchoredPosition = position;

            this.type = type;
            switch (type)
            {
                case BlockType.GRAY:
                    image.color = Color.gray;
                    break;
                case BlockType.BLUE:
                    image.color = Color.blue;
                    break;
                case BlockType.GREEN:
                    image.color = Color.green;
                    break;
                case BlockType.RED:
                    image.color = Color.red;
                    break;
                default:
                    break;
            }
        }

        public BlockType GetBlockType() => type;

    }

}