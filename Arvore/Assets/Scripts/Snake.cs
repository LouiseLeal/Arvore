using System;
using System.Collections.Generic;
using UnityEngine;

//All warning were verified 
#pragma warning disable CS0649
namespace Snake
{

    public struct LastSnakeMoviment
    {
        public SnakeDirection lastDirection;
        //public Position snakeHeadPosiiton;
        public Position snakeTailPosition;
        public int snakeSize;
    }

    public class Snake : MonoBehaviour
    {
        [SerializeField] private GameObject snakeTilePrefab;
         protected List<SnakeTile> snake = new List<SnakeTile>();

        protected float inverseSpeed;

        int currentSize;
        protected float nextMoveTime;

        protected bool canCheckInput = false;

        protected SnakeDirection currentDirection;

        int Blockseaten = 0;

        Position snakeLastTileInfo;

        LastSnakeMoviment lastMoviment;

        //TODO create a snake base
        protected virtual void Update()
        {
            if (snake == null)
                return;

            CheckForInput();

            nextMoveTime -= Time.deltaTime;

            if (nextMoveTime < 0)
            {
                CheckForMove();
            }
        }


        void CheckForInput()
        {

            if (canCheckInput)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("SPACE");
                    canCheckInput = false;
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    Rotate(clockwise: true);
                    canCheckInput = false;
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    Rotate(clockwise: false);
                    canCheckInput = false;
                }
            }
        }

        protected virtual void CheckForMove()
        {
            Move();
            CheckForBlock();
            nextMoveTime = inverseSpeed;
            canCheckInput = true;
            StoreLastMoviment();
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

            this.inverseSpeed = speed;
            nextMoveTime = speed;

            canCheckInput = true;

            currentDirection = SnakeDirection.RIGHT;

            var newSnakeTile = (Instantiate(snakeTilePrefab, this.transform)
                                        as GameObject).GetComponent<SnakeTile>();


            //Define an rando y position of snake
            var snakeTilePositionY = UnityEngine.Random.Range(1, arenaHeight - 1);
            //Consider not position on wall;
            var snakeTilePositionX = initialTileSize;
            var snakeTilePosition = new Vector2(snakeTilePositionX, snakeTilePositionY);

            Position snakeTilePosition2;
            snakeTilePosition2.x = snakeTilePositionX;
            snakeTilePosition2.y = snakeTilePositionY;

            if(!GameManager.Instance.IsEmptyArenaTile(snakeTilePosition2.x, snakeTilePosition2.y))
            {
                snakeTilePosition2.y = GameManager.Instance.GetNextVerticalPosition(snakeTilePosition2).y;
            }

            //Todo change
            snakeTilePosition[0] = snakeTilePosition2.x;
            snakeTilePosition[1] = snakeTilePosition2.y;

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

        public virtual void  Move()
        {
            if (currentDirection != snake[0].currentDirection)
                snake[0].SetRotation(currentDirection);


            var lastSnakePart = currentSize - 1;

            //Storege last position for use it on gayr effect block
            snakeLastTileInfo.x = snake[lastSnakePart].x;
            snakeLastTileInfo.y = snake[lastSnakePart].y;

            GameManager.Instance.SetArenaTileState(snake[lastSnakePart].x,
                                    snake[lastSnakePart].y, ArenaTileState.EMPTY);


            for (int i = lastSnakePart; i > 0; i--)
            {
                snake[i].CopyValue(snake[i - 1]);
            }

            switch (snake[0].currentDirection)
            {
                case SnakeDirection.UP:
                    snake[0].SetPosition(snake[0].x, snake[0].y - 1, true);
                    break;
                case SnakeDirection.RIGHT:
                    snake[0].SetPosition(snake[0].x + 1, snake[0].y, true);
                    break;
                case SnakeDirection.DOWN:
                    snake[0].SetPosition(snake[0].x, snake[0].y + 1, true);
                    break;
                case SnakeDirection.LEFT:
                    snake[0].SetPosition(snake[0].x - 1, snake[0].y, true);
                    break;
            }
        }

        protected void StoreLastMoviment()
        {
            lastMoviment.lastDirection = currentDirection;
            //lastMoviment.snakeHeadPosiiton = snake[0].GetPosition(0);
            lastMoviment.snakeTailPosition = snake[currentSize-1].GetPosition(0);
            lastMoviment.snakeSize = currentSize;
        }


        protected void CheckForBlock()
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
                        BlueEffect();
                        break;
                    case BlockType.GREEN:
                        GrayEffect();
                        GreenEffect();
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
                    currentDirection = SnakeDirection.UP;
            }
            else
            {
                if (currentDirection != SnakeDirection.UP)
                    currentDirection -= 1;
                else
                    currentDirection = SnakeDirection.LEFT;

            }

            //Debug.Log("Rotate " + currentDirection);
        }


        #region BlocksEffects
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

        void GreenEffect()
        {
            Debug.Log("GreenEffect");
            inverseSpeed -= GameManager.Instance.gameData.SpeedIncrease;
        }

        void BlueEffect()
        {
            Debug.Log("Blue effect");
            GameManager.Instance.ReturnTime();
        }


        public void ReturnTime()
        {
            //Reset next moviment time for everyone
            nextMoveTime = inverseSpeed;

            //Because the snakes will return a tile 
            //Is needed to set this tile as empty
            GameManager.Instance.SetArenaTileState(snake[0].x, snake[0].y,
                                                         ArenaTileState.EMPTY);

            currentDirection = lastMoviment.lastDirection;
            for (int i = 0; i < currentSize - 1; i++)
            {
                snake[i].CopyValue(snake[i + 1]);
            }

            if(currentSize - lastMoviment.snakeSize > 0)
            {
                snake.Remove(snake[currentSize-1]);
            }
            else
            {
                snake[currentSize - 1].SetPosition(
                                    lastMoviment.snakeTailPosition.x,
                                    lastMoviment.snakeTailPosition.y);
            }
        }
        #endregion
    }
}
#pragma warning restore CS0649