using UnityEngine;
using System.Collections;



namespace Snake {

    public class SnakeAI : Snake
    {

        public override void CreateSnake(int initialTileSize, int arenaHeight, Vector2 TileSize, float speed)
        {
            base.CreateSnake(initialTileSize, arenaHeight, TileSize, speed);
            SetSnakeAI();
        }

        protected override void Update()
        {
            if (snakeTiles == null && !isActiveAndEnabled)
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


        void ChangeDirection()
        {
           

            var position = new Position();
            position.x = snakeTiles[0].x;
            position.y = snakeTiles[0].y;

            var targetPosition = GameManager.Instance.GetNearBlockPosition(position);

            if (!HorizontalyDirection(position, targetPosition))
            {
                if(!VerticalyDirection(position, targetPosition))
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


    }

}