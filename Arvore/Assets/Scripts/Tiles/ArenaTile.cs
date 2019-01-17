using System;
using UnityEngine;
using UnityEngine.UI;

namespace Snake
{
  
    public class ArenaTile : Tile
    {
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

        public Vector2 GetCanvasPosition()
        {
            return rectTransform.anchoredPosition;
        }

        public void SetCanvasPosiiton(Vector2 position)
        {
            rectTransform.anchoredPosition = position;
        }
    }
}
