using System.Collections.Generic;
using UnityEngine;


namespace Snake
{

    public class Snake : MonoBehaviour
    {
        [SerializeField] private GameObject snakeTilePrefab;
        List<SnakeTile> snake = new List<SnakeTile>();

        float speed;

        int currentSize;
        float nextMoveTime;

        bool canCheckInput = false;

        SnakeDirection currentDirection;

        private void Update()
        {
            if (snake != null)
                return;

            if (canCheckInput )  
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("SPACE");
                    canCheckInput = false;
                }

                if (Input.GetKeyDown(KeyCode.D)){
                    Rotate(clockwise: true);
                    canCheckInput = false;
                }
                else if (Input.GetKeyDown(KeyCode.A)){
                    Rotate(clockwise: false);
                    canCheckInput = false;
                }

             
            }

            nextMoveTime -= Time.deltaTime;

            if (nextMoveTime < 0)
            {
                Move();
                nextMoveTime = speed;
                canCheckInput = true;
            }
        }


        public void CreateSnake(int initialTileSize, int arenaHeight, Vector2 TileSize, float speed)
        {
            snake = new List<SnakeTile>();

            this.speed = speed;
            nextMoveTime = speed;

            canCheckInput = true;

            currentDirection = SnakeDirection.RIGHT;

            var newSnakeTile = (Instantiate(snakeTilePrefab, this.transform)
                                        as GameObject).GetComponent<SnakeTile>();


            //Define an rando y position of snake
            var snakeTilePositionY = Random.Range(1, arenaHeight - 1);
            //Consider not position on wall;
            var snakeTilePositionX = initialTileSize;
            var snakeTilePosition = new Vector2(snakeTilePositionX, snakeTilePositionY);

            currentSize = initialTileSize;

            newSnakeTile.SetSnake(snakeTilePosition, TileSize, true);

            snake.Add(newSnakeTile);

            for (int i = 1; i < initialTileSize; i++)
            {
                newSnakeTile = (Instantiate(snakeTilePrefab, this.transform)
                                        as GameObject).GetComponent<SnakeTile>();

                snakeTilePosition.x = snakeTilePositionX - i;

                newSnakeTile.SetSnake(snakeTilePosition, TileSize, false);

                snake.Add(newSnakeTile);
            }
        }

        void Move()
        {
            for (int i = currentSize - 1; i > 0; i--)
            {
                snake[i].CopyValue(snake[i - 1]);
            }

            if (currentDirection != snake[0].currentDirection)
                snake[0].SetRotation(currentDirection);

            switch (snake[0].currentDirection)
            {
                case SnakeDirection.TOP:
                    snake[0].SetPosition(snake[0].x, snake[0].y + 1);
                    break;
                case SnakeDirection.RIGHT:
                    snake[0].SetPosition(snake[0].x + 1, snake[0].y);
                    break;
                case SnakeDirection.DOWN:
                    snake[0].SetPosition(snake[0].x, snake[0].y - 1);
                    break;
                case SnakeDirection.LEFT:
                    snake[0].SetPosition(snake[0].x - 1, snake[0].y);
                    break;
            }


        }


        void Rotate(bool clockwise)
        {

            if (clockwise)
            {
                if (currentDirection != SnakeDirection.LEFT)
                    currentDirection += 1;
                else
                    currentDirection = SnakeDirection.TOP;
            }
            else
            {
                if (currentDirection != SnakeDirection.TOP)
                    currentDirection -= 1;
                else
                    currentDirection = SnakeDirection.LEFT;

            }

            Debug.Log("Rotate " + currentDirection);
        }
    }

}