using UnityEngine;
using UnityEngine.UI;

namespace Snake
{
  
    public class ArenaTile : MonoBehaviour
    {
        [SerializeField] SpriteData spriteData;

        [SerializeField] Image image;
        [SerializeField] public RectTransform rectTransform;


        bool isWall = false;

        private void Awake()
        {
            image.sprite = spriteData.MapTile;
        }

        public void SetWall()
        {
            isWall = true;
            image.sprite = spriteData.MapWall;
            gameObject.AddComponent<Collider2D>();
            //Todo set size if needed

        }

    }
}
