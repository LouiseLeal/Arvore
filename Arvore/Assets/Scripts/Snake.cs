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
        protected List<SnakeTile> snakeTiles = new List<SnakeTile>();

        protected float inverseSpeed;

        int currentSize;
        protected float nextMoveTime;

        protected bool canCheckInput = false;

        protected SnakeDirection currentDirection;

        int Blockseaten = 0;

        Position snakeLastTileInfo;

        LastSnakeMoviment lastMoviment;
        private bool isAI = false;

        public bool isActive;

        //TODO create a snake base
        protected virtual void Update()
        {
            if (snakeTiles == null || !isActive)
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

        protected void CheckForMove()
        {

            if (!Move())
            {
                GameManager.Instance.SnakeDie(this);
                return;
            }
            CheckForBlock();
            nextMoveTime = inverseSpeed;
            canCheckInput = true;
            StoreLastMoviment();
        }


        public virtual void CreateSnake(int initialTileSize, int arenaHeight, Vector2 TileSize, float speed)
        {

            //TODO do it in a more efficiente
            if (snakeTiles.Count != 0)
            {
                for (int i = 0; i < snakeTiles.Count; i++)
                {
                    Destroy(snakeTiles[i].gameObject);
                }
            }

            snakeTiles = new List<SnakeTile>();

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

            if (!GameManager.Instance.IsEmptyArenaTile(snakeTilePosition2.x, snakeTilePosition2.y))
            {
                snakeTilePosition2.y = GameManager.Instance.GetNextVerticalPosition(snakeTilePosition2).y;
            }

            //Todo change
            snakeTilePosition[0] = snakeTilePosition2.x;
            snakeTilePosition[1] = snakeTilePosition2.y;

            currentSize = initialTileSize;

            newSnakeTile.SetSnake(snakeTilePosition, TileSize, true);

            snakeTiles.Add(newSnakeTile);

            for (int i = 1; i < initialTileSize; i++)
            {
                newSnakeTile = (Instantiate(snakeTilePrefab, this.transform)
                                        as GameObject).GetComponent<SnakeTile>();

                snakeTilePosition.x = snakeTilePositionX - i;

                newSnakeTile.SetSnake(snakeTilePosition, TileSize, false);

                snakeTiles.Add(newSnakeTile);
            }

            isActive = true;
        }

        public virtual bool Move()
        {
            if (currentDirection != snakeTiles[0].currentDirection)
                snakeTiles[0].SetRotation(currentDirection);


            var lastSnakePart = currentSize - 1;

            //Storege last position for use it on gayr effect block
            snakeLastTileInfo.x = snakeTiles[lastSnakePart].x;
            snakeLastTileInfo.y = snakeTiles[lastSnakePart].y;

            GameManager.Instance.SetArenaTileState(snakeTiles[lastSnakePart].x,
                                    snakeTiles[lastSnakePart].y, ArenaTileState.EMPTY);


            for (int i = lastSnakePart; i > 0; i--)
            {
                snakeTiles[i].CopyValue(snakeTiles[i - 1]);
            }

            //Check if tile head could set position without 
            //collider with wall or snakeTile
            bool result = true;

            switch (snakeTiles[0].currentDirection)
            {
                case SnakeDirection.UP:
                    result = snakeTiles[0].SetPosition(snakeTiles[0].x, snakeTiles[0].y - 1, true);
                    break;
                case SnakeDirection.RIGHT:
                    result = snakeTiles[0].SetPosition(snakeTiles[0].x + 1, snakeTiles[0].y, true);
                    break;
                case SnakeDirection.DOWN:
                    result = snakeTiles[0].SetPosition(snakeTiles[0].x, snakeTiles[0].y + 1, true);
                    break;
                case SnakeDirection.LEFT:
                    result = snakeTiles[0].SetPosition(snakeTiles[0].x - 1, snakeTiles[0].y, true);
                    break;
            }


            return result;

        }

        protected void StoreLastMoviment()
        {
            lastMoviment.lastDirection = currentDirection;
            //lastMoviment.snakeHeadPosiiton = snake[0].GetPosition(0);
            lastMoviment.snakeTailPosition = snakeTiles[currentSize - 1].GetPosition(0);
            lastMoviment.snakeSize = currentSize;
        }


        protected void CheckForBlock()
        {
            ArenaTileState checkResult = GameManager.Instance.CheckTileForSnake
                            (snakeTiles[0].x, snakeTiles[0].y, out BlockType blockType);

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

            snakeTiles.Add(newSnakeTile);
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
            GameManager.Instance.SetArenaTileState(snakeTiles[0].x, snakeTiles[0].y,
                                                         ArenaTileState.EMPTY);

            currentDirection = lastMoviment.lastDirection;
            for (int i = 0; i < currentSize - 1; i++)
            {
                snakeTiles[i].CopyValue(snakeTiles[i + 1]);
            }

            if (currentSize - lastMoviment.snakeSize > 0)
            {
                snakeTiles.Remove(snakeTiles[currentSize - 1]);
            }
            else
            {
                snakeTiles[currentSize - 1].SetPosition(
                                    lastMoviment.snakeTailPosition.x,
                                    lastMoviment.snakeTailPosition.y);
            }
        }

        //TODO Use pool like way to destroy (active) new snake
        public void Die()
        {
            Debug.Log("Die " + name);
            if (snakeTiles == null)
                return;

            for (int i = 0; i < snakeTiles.Count; i++)
            {
                //Destroy all snakeTile gameobjects
                GameManager.Instance.SetArenaTileState(snakeTiles[i].x, snakeTiles[i].y,
                                                         ArenaTileState.EMPTY);
                Destroy(snakeTiles[i].gameObject);
            }

            snakeTiles = null;
            this.enabled = false;

            Destroy(gameObject);
        }

        #endregion

        public void SetSnakeAI()
        {
            isAI = true;
        }

        public bool IsSnakeAI()
        {
            return isAI;
        }
    }
}
#pragma warning restore CS0649