using UnityEngine;

namespace Snake
{
    public class SnakeTile : Tile
    {

        bool isHead = false;

        public void Start()
        {
            image.sprite = spriteData.SnakeBody;
        }

        public void SetSnake(Vector2 position, Vector2 tileSize, bool isHead)
        {
            this.isHead = isHead;

            if (isHead)
                image.sprite = spriteData.SnakeHead;
            else
                image.sprite = spriteData.SnakeBody;


            SetTileSize(tileSize);
            SetPosition((int)position.x, (int)position.y);
            Debug.Log(position.x + " " + isHead);
        }


        public void SetPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
            rectTransform.anchoredPosition = GameManager.Instance.GetCanvasPosition(x, y);
        }
    }
}
