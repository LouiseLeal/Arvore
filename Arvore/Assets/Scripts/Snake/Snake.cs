using UnityEngine;
using System.Collections.Generic;

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

        int BlocksEaten = 0;

        Position snakeLastTileInfo;

        LastSnakeMoviment lastMoviment;
        protected bool isAI = false;

        protected bool isActive = false;

        //Snakes Preset
        [SerializeField] SnakesPresetsData snakesPresetsData;
        protected static List<int> usedSnakesPreset = new List<int>();
        protected int indexSelectedColor;

        #region GetterSetter
        public void SetSnakeAI()
        {
            isAI = true;
        }

        public bool IsSnakeAI()
        {
            return isAI;
        }

        public void SetActive(bool active)
        {
            isActive = active;
        }

        public bool IsActive()
        {
            return isActive;
        }
        #endregion

        protected virtual void Update() { }


        public virtual void CreateSnake(int initialTileCount, int arenaHeight, Vector2 TileSize, float speed)
        {

            //TODO do it in a more efficiente
            if (snakeTiles.Count != 0)
            {
                for (int i = 0; i < snakeTiles.Count; i++)
                {
                    Destroy(snakeTiles[i].gameObject);
                }
            }

            //Set snake config
            snakeTiles = new List<SnakeTile>();

            this.inverseSpeed = speed;
            nextMoveTime = speed;

            currentDirection = SnakeDirection.RIGHT;


            //Init creating snakes tiles
            var newSnakeTile = (Instantiate(snakeTilePrefab, this.transform)
                                        as GameObject).GetComponent<SnakeTile>();


            //Define an rando y position of snake
            var snakeTilePositionY = UnityEngine.Random.Range(1, arenaHeight - 1);
            //Consider not position on wall;
            var snakeTilePositionX = initialTileCount;

            Position snakeTilePosition;
            snakeTilePosition.x = snakeTilePositionX;
            snakeTilePosition.y = snakeTilePositionY;

            //If random tile is not available find the next one
            if (!GameManager.Instance.IsEmptyArenaTile(snakeTilePosition.x, snakeTilePosition.y))
            {
                snakeTilePosition.y = GameManager.Instance.GetNextVerticalPosition(snakeTilePosition).y;
            }

            currentSize = initialTileCount;

            newSnakeTile.SetValidSnakeTile(snakeTilePosition, TileSize, true);
            GameManager.Instance.SetArenaTileState(snakeTilePosition.x, snakeTilePosition.y, ArenaTileState.SNAKE);
            snakeTiles.Add(newSnakeTile);

            for (int i = 1; i < initialTileCount; i++)
            {
                newSnakeTile = (Instantiate(snakeTilePrefab, this.transform)
                                        as GameObject).GetComponent<SnakeTile>();

                snakeTilePosition.x = snakeTilePositionX - i;

                newSnakeTile.SetValidSnakeTile(snakeTilePosition, TileSize, false);
                GameManager.Instance.SetArenaTileState(snakeTilePosition.x, snakeTilePosition.y, ArenaTileState.SNAKE);

                snakeTiles.Add(newSnakeTile);
            }

        }


        protected void CheckForMove()
        {
            if (!Move())
            {
                GameManager.Instance.SnakeDie(this);
                return;
            }
            canCheckInput = true;
            CheckForBlock();
            nextMoveTime = inverseSpeed;
            StoreLastMoviment();
        }

        public virtual void SelectSnakePreset() { }

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

            if (snakeTiles != null)
            {
                Debug.Log("StoreLastMoviment " + name);
                lastMoviment.lastDirection = currentDirection;
                //lastMoviment.snakeHeadPosiiton = snake[0].GetPosition(0);
                lastMoviment.snakeTailPosition = snakeTiles[currentSize - 1].GetPosition(0);
                lastMoviment.snakeSize = currentSize;
            }
        }

        protected void CheckForBlock()
        {
            ArenaTileState checkResult = GameManager.Instance.CheckTileForSnake
                            (snakeTiles[0].x, snakeTiles[0].y, out BlockType blockType);

            if (checkResult == ArenaTileState.BLOCK)
            {

                BlocksEaten++;
                Debug.Log("eat " + BlocksEaten);

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
                        GrayEffect();
                        RedEffect();
                        break;
                    default:
                        break;
                }
            }

            #region BlocksEffects
            //Enlarge Snake by one tile
            void GrayEffect()
            {
                Debug.Log("Gray effect");
                currentSize++;

                Position position;
                position.x = snakeLastTileInfo.x;
                position.y = snakeLastTileInfo.y;


                var newSnakeTile = Instantiate(snakeTilePrefab, this.transform).
                                                        GetComponent<SnakeTile>();

                //This tile is always valid because it replace the current tail tile,
                //so a valid tile
                newSnakeTile.SetValidSnakeTile(position,
                                               GameManager.Instance.tileSize, false);

                newSnakeTile.TintTile(snakesPresetsData.colors[indexSelectedColor]);
                snakeTiles.Add(newSnakeTile);
            }

            void GreenEffect()
            {
                Debug.Log("GreenEffect");
                inverseSpeed -= GameManager.Instance.gameData.speedIncrease;
            }

            void BlueEffect()
            {
                Debug.Log("Blue effect");
                GameManager.Instance.ReturnTime();
            }

            void RedEffect()
            {
                GameManager.Instance.FireBullet(this);
            }
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
        #endregion


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

            //reset list
            snakeTiles = new List<SnakeTile>();
            this.enabled = false;

            Destroy(gameObject);
        }
    


        public Position GetHeadPosition()
        {
            Position position;
            position.x = snakeTiles[0].x;
            position.y = snakeTiles[0].y;
            return position;
        }

        public Snake IsOtherSnakeInRange(Snake other, int range)
        {
            //Compare Heads distance in tiles
            bool snakeInRangeX = Mathf.Abs(this.snakeTiles[0].x -
                                               other.snakeTiles[0].x) == range;
            bool snakeInRangeY = Mathf.Abs(this.snakeTiles[0].y -
                                               other.snakeTiles[0].y) == range;

            if (snakeInRangeX && snakeInRangeY)
                return other;

            return null;
        }

        protected void Rotate(bool clockwise)
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

        #region SnakePreset
        protected void TintSnake(int index)
        {
            for (int i = 0; i < snakeTiles.Count; i++)
            {
                snakeTiles[i].TintTile(snakesPresetsData.colors[index]);
            }
        }

        public static void ResetUsedColors()
        {
            usedSnakesPreset = new List<int>();
        }

        protected bool IsColorIndexUsed(int indexColor)
        {
            for (int i = 0; i < usedSnakesPreset.Count; i++)
            {
                if (usedSnakesPreset[i] == indexColor)
                    return true;
            }

            return false;
        }

        #endregion
    }
}
