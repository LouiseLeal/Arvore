﻿using System;
using UnityEngine;

namespace Snake
{
    public enum SnakeDirection
    {
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3,
        INVALID
    }

    public class SnakeTile : Tile
    {
        //Because the original rotation of the sprite the default direction is top
        public SnakeDirection currentDirection = SnakeDirection.UP;
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
            SetRotation(SnakeDirection.RIGHT);

            //Debug.Log(position.x + " " + isHead);
        }

        public void SetRotation(SnakeDirection direction)
        {
            currentDirection = direction;

            switch (currentDirection)
            {
                case SnakeDirection.UP:
                    rectTransform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case SnakeDirection.RIGHT:
                    rectTransform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case SnakeDirection.DOWN:
                    rectTransform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case SnakeDirection.LEFT:
                    rectTransform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                default:
                    break;
            }
        }

        public void SetPosition(int x, int y)
        {
            //Debug.Log("name" + name);
            if(x == 0 || y == 0 || 
                x == GameManager.Instance.gameData.arenaWidth-1 ||
                y == GameManager.Instance.gameData.arenaHeight-1)
            {
                GameManager.Instance.GameOver();
                return;
            }

            this.x = x;
            this.y = y;
            rectTransform.anchoredPosition = GameManager.Instance.GetCanvasPosition(x, y);
        }

        public void CopyValue(SnakeTile snakeTile)
        {
            if (currentDirection != snakeTile.currentDirection)
            {
                SetRotation(snakeTile.currentDirection);
            }

            SetPosition(snakeTile.x, snakeTile.y);
        }

        //ToDO refactor to use struct position
        public Vector2 GetPosition()
        {
            return new Vector2(x,y);
        }

        //TODO refactore
        public Position GetPosition(int none)
        {
            Position position;
            position.x = x;
            position.y = y;
            return position;
        }
    }
}
