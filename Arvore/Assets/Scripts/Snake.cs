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

        int Blockseaten = 0;

        Posiiton snakeLastTileInfo;


        private void Update()
        {
            if (snake == null)
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
                CheckForBlock();
                nextMoveTime = speed;
                canCheckInput = true;
            }
        }


        public void CreateSnake(int initialTileSize, int arenaHeight, Vector2 TileSize, float speed)
        {

            //TODO do it in a more efficiente
            if(snake.Count != 0)
            {
                for (int i = 0; i < snake.Count; i++)
                {
                    Destroy(snake[i].gameObject);
                }
            }

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
            //Check if the current tile has something;

            var lastSnakePart = currentSize - 1;

            //Todo see this
            //Storege last position for use it on gayr effect block
            snakeLastTileInfo.x = snake[lastSnakePart].x;
            snakeLastTileInfo.y = snake[lastSnakePart].y;

            GameManager.Instance.SetArenaTileState(snake[lastSnakePart].x, 
                                    snake[lastSnakePart].y, ArenaTileState.EMPTY);


            for (int i = lastSnakePart; i > 0; i--)
            {
                snake[i].CopyValue(snake[i-1]);
            }

        }

        void CheckForBlock()
        {
            ArenaTileState checkResult = GameManager.Instance.CheckTileForSnake
                            (snake[0].x, snake[0].y, out BlockType blockType);

            if (checkResult == ArenaTileState.BLOCK)
            {

                Blockseaten++;
                Debug.Log("eat " + Blockseaten);

                switch (blockType)
                {
                    case BlockType.INACTIVE:
                        Debug.Log("ERROR");
                        break;
                    case BlockType.GRAY:
                        GrayEffect();
                        break;
                    case BlockType.BLUE:
                        break;
                    case BlockType.GREEN:
                        break;
                    case BlockType.RED:
                        break;
                    default:
                        break;
                }
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

            //Debug.Log("Rotate " + currentDirection);
        }


        //Enlarge Snake by one tile
        void GrayEffect()
        {
            Debug.Log("Gray effect");
            currentSize++;

            Vector2 position = new Vector2(snakeLastTileInfo.x, snakeLastTileInfo.y);


            var newSnakeTile = Instantiate(snakeTilePrefab, this.transform).
                                                    GetComponent<SnakeTile>();
            newSnakeTile.SetSnake(position, GameManager.Instance.tileSize, false);

            snake.Add(newSnakeTile);
        }
    }
}