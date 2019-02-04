using UnityEngine;

namespace Snake
{

    public class SnakeAI : Snake
    {

        public override void CreateSnake(int initialTileCount, int arenaHeight, Vector2 TileSize, float speed)
        {
            Debug.Log("Create an snake AI");

            base.CreateSnake(initialTileCount, arenaHeight, TileSize, speed);
            SetSnakeAI();
            ColorSnake();
            isActive = true;
        }



        protected override void Update()
        {
            if (snakeTiles == null || !isActive)
                return;

            nextMoveTime -= Time.deltaTime;

            if (nextMoveTime < 0)
            {
                CheckForMove();
            }
        }

        public override bool Move()
        {

            ChangeDirection();
            return base.Move();
        }


        #region SnakeAILogic
        void ChangeDirection()
        {

            var position = new Position();
            position.x = snakeTiles[0].x;
            position.y = snakeTiles[0].y;

            var targetPosition = GameManager.Instance.GetNearBlockPosition(position);

            if (!HorizontalyDirection(position, targetPosition))
            {
                if (!VerticalyDirection(position, targetPosition))
                {
                    if (currentDirection == SnakeDirection.INVALID)
                        Debug.Log("Inavlid");
                    else
                        //The snakes head is on top of target this will be 
                        //dealed with in follow methods;
                        return;

                }
            }
        }


        bool VerticalyDirection(Position position, Position targetPosition)
        {
            //Turn down
            if (targetPosition.y - position.y > 0)
            {
                if (currentDirection != SnakeDirection.DOWN)
                {
                    if (currentDirection == SnakeDirection.RIGHT ||
                                      currentDirection == SnakeDirection.LEFT)
                    {
                        currentDirection = SnakeDirection.DOWN;
                        return true;
                    }

                    var auxDirection = TurnHorizontaly(position);

                    if (currentDirection == SnakeDirection.INVALID)
                        return false;

                    currentDirection = auxDirection;
                    return true;
                }
                else if (GameManager.Instance.IsEmptyArenaTile(position.x, position.y + 1))
                {
                    //Continue in the same direction
                    return true;
                }
            }
            //Turn up
            else if (targetPosition.y - position.y < 0)
            {
                if (currentDirection != SnakeDirection.UP)
                {
                    if (currentDirection == SnakeDirection.RIGHT ||
                                      currentDirection == SnakeDirection.LEFT)
                    {
                        currentDirection = SnakeDirection.UP;
                        return true;
                    }

                    var auxDirection = TurnHorizontaly(position);

                    if (currentDirection == SnakeDirection.INVALID)
                        return false;

                    currentDirection = auxDirection;
                    return true;
                }

                else if (GameManager.Instance.IsEmptyArenaTile(position.x, position.y - 1))
                {
                    //Continue in the same direction
                    return true;
                }
            }
            return false;
        }

        bool HorizontalyDirection(Position position, Position targetPosition)
        {
            //Move left
            if (targetPosition.x - position.x < 0)
            {
                if (currentDirection != SnakeDirection.LEFT)
                {
                    if (currentDirection == SnakeDirection.UP ||
                                        currentDirection == SnakeDirection.DOWN)
                    {
                        currentDirection = SnakeDirection.LEFT;
                        return true;
                    }

                    var auxDirection = TurnVerticaly(position);

                    if (currentDirection == SnakeDirection.INVALID)
                        return false;

                    currentDirection = auxDirection;
                    return true;
                }
                else if (GameManager.Instance.IsEmptyArenaTile(position.x - 1, position.y))
                {
                    //Continue in the same direction
                    return true;
                }

            }
            //Move right
            else if (targetPosition.x - position.x > 0)
            {
                if (currentDirection != SnakeDirection.RIGHT)
                {
                    if (currentDirection == SnakeDirection.UP ||
                                       currentDirection == SnakeDirection.DOWN)
                    {
                        currentDirection = SnakeDirection.RIGHT;
                        return true;
                    }

                    var auxDirection = TurnVerticaly(position);

                    if (currentDirection == SnakeDirection.INVALID)
                        return false;

                    currentDirection = auxDirection;
                    return true;
                }

                else if (GameManager.Instance.IsEmptyArenaTile(position.x + 1, position.y))
                {
                    //Continue in the same direction
                    return true;
                }
            }
            return false;
        }

        SnakeDirection TurnVerticaly(Position position)
        {
            if (GameManager.Instance.IsEmptyArenaTile(position.x, position.y + 1))
            {
                return SnakeDirection.UP;
            }
            else if (GameManager.Instance.IsEmptyArenaTile(position.x, position.y - 1))
            {
                return SnakeDirection.DOWN;
            }

            Debug.LogWarning("Snake is Trapped Verticaly");

            return SnakeDirection.INVALID;
        }

        SnakeDirection TurnHorizontaly(Position position)
        {
            if (GameManager.Instance.IsEmptyArenaTile(position.x - 1, position.y))
            {
                return SnakeDirection.LEFT;
            }
            else if (GameManager.Instance.IsEmptyArenaTile(position.x + 1, position.y))
            {
                return SnakeDirection.RIGHT;
            }

            Debug.LogWarning("Snake is Trapped hotizontaly");

            return SnakeDirection.INVALID;
        }
        #endregion

        #region SnakePreset

        private void ColorSnake()
        {
            indexSelectedColor = -1;
            var indexColor = Random.Range(0, 35);

            if (IsColorIndexUsed(indexColor))
            {
                //The max colors is 26 because of the max 
                //combinations  keys
                
                for (int i = indexColor+1; i < 36; i++)
                {
                    if (IsColorIndexUsed(i))
                    {
                        indexSelectedColor = i;
                        break;
                    }
                }

                if (indexSelectedColor == -1)
                {
                    for (int i = 0; i < indexColor; i++)
                    {
                        if (IsColorIndexUsed(i))
                        {
                            indexSelectedColor = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                indexSelectedColor = indexColor;
            }

            TintSnake(indexSelectedColor);
            usedSnakesPreset.Add(indexSelectedColor);
        }
        #endregion
    }
}