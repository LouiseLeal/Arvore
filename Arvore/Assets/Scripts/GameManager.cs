using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//All warning were verified 
#pragma warning disable CS0649

namespace Snake
{

    public struct Position
    {
        public int x;
        public int y;

    }

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] public GameData gameData;
        [SerializeField] private GameObject MapTilePrefab;
        [SerializeField] private Transform ArenaContainer;
        [SerializeField] private BlocksSpawnManager BlocksManager;



        ArenaTile[,] arena;

        [HideInInspector] public Vector2 tileSize;

        [Header("Snakes Settings")]
        [SerializeField] private Snake snakePrefab;
        [SerializeField] private SnakeAI snakeAIPrefab;
        [SerializeField] Transform snakeContainer;
        List<Snake> snakes = new List<Snake>();
        int snakeAmount;

        #region UnityMethods
        private void Awake()
        {
            snakeAmount = gameData.snakesPlayerAmount;

            //Todo create an method to calculate it based on screen size
            tileSize = new Vector2(gameData.TileSize, gameData.TileSize);

            if (gameData == null)
                gameData = Resources.Load<GameData>("GameDataDefault");

            arena = new ArenaTile[gameData.arenaWidth, gameData.arenaHeight];

            BlocksManager.SetBlockChances(gameData.grayBlock, gameData.greenBlock,
                                            gameData.blueBlock, gameData.redBlock);


            BlocksManager.CreateBlockPool(5, tileSize);

            CreateArena();

        }

        private void Start()
        {
            SetBlock();

            //Declare before for the avoid garbage collector
            Snake newSnake;
            SnakeAI newSnakeAI;

            for (int i = 0; i < snakeAmount; i++)
            {
                newSnake = Instantiate(snakePrefab, snakeContainer).GetComponent<Snake>();
                newSnake.CreateSnake(gameData.initialSnakeSize, gameData.arenaHeight, tileSize, gameData.snakeSpeed);
                snakes.Add(newSnake);

                newSnakeAI = Instantiate(snakeAIPrefab, snakeContainer).GetComponent<SnakeAI>();
                newSnakeAI.CreateSnake(gameData.initialSnakeSize, gameData.arenaHeight, tileSize, gameData.snakeSpeed);
                snakes.Add(newSnakeAI);
            }
        }


        #endregion


        #region GameSetting
        //Create arena programaticaly
        private void CreateArena()
        {
            //set variables before for the avoid garbage collection
            GameObject newGameObject;
            ArenaTile newTile;
            Vector2 newCanvasPosition = Vector2.zero;

            for (int y = 0; y < gameData.arenaHeight; y++)
            {
                for (int x = 0; x < gameData.arenaWidth; x++)
                {
                    newGameObject = Instantiate(MapTilePrefab, ArenaContainer) as GameObject;

                    //Use string build for more optimized code
                    StringBuilder tileName = new StringBuilder("arena ", 30);
                    tileName.Append(x.ToString());
                    tileName.Append(" ");
                    tileName.Append(y.ToString());
                    newGameObject.name = tileName.ToString();

                    newTile = newGameObject.GetComponent<ArenaTile>();
                    newTile.SetTileSize (tileSize);

                    newCanvasPosition.x = tileSize.x * x;
                    newCanvasPosition.y = -tileSize.y * y;
                    newTile.SetCanvasPosiiton(newCanvasPosition);

                    //If the tile is on board, the tile will be an wall
                    if (y == 0 || y == gameData.arenaHeight - 1 || x == 0 || x == gameData.arenaWidth - 1)
                        newTile.SetWall();

                    arena[x, y] = newTile;
                }
            }
        }

        public Position GetNextVerticalPosition(Position position)
        {
            //Check for greater y empty value
            for (int i = position.y + 1; i < gameData.arenaHeight - 1; i++)
            {
                if(IsEmptyArenaTile(position.x, i))
                {
                    position.y = i;
                    return position;
                }
            }

            //Check for smaller y empty value

            for (int i = 0; i < position.y -1; i++)
            {
                if (IsEmptyArenaTile(position.x, i))
                {
                    position.y = i;
                    return position;
                }
            }

            Debug.LogError("Could not find and vertical value");
            return position;

        }

        public Vector2 GetCanvasPosition(int x,int y)
        {
            return arena[x, y].GetCanvasPosition();
        }

       
        private BlockType SetBlock()
        {
            //Find a random and valid tile to place the new block
            Position random;
            random.x = UnityEngine.Random.Range(1, gameData.arenaWidth - 2);
            random.y = UnityEngine.Random.Range(1, gameData.arenaHeight - 2);

            if (arena[random.x, random.y].GetArenaTileState() == ArenaTileState.EMPTY)
            {

                Block block = BlocksManager.EnableBlock(random,arena[random.x,
                                              random.y].GetCanvasPosition());
                if (block == null)
                    return BlockType.INACTIVE;

                arena[random.x, random.y].
                       ChangeArenaTileState(ArenaTileState.BLOCK, block);

                return block.type;
            }
            //To reduce an if in a while loop, It is divided in two fors;
            Position newRandom;
            newRandom.x = random.x + 1;
            newRandom.y = random.y + 1;
            for (int x = newRandom.x; x < gameData.arenaWidth - 1; x++)
            {
                for (int y = newRandom.y; y < gameData.arenaHeight - 1; y++)
                {
                    newRandom.x = x;
                    newRandom.y = y;
                    if (arena[newRandom.x, newRandom.y].GetArenaTileState() == ArenaTileState.EMPTY)
                    {

                        var block = BlocksManager.EnableBlock(newRandom,arena[newRandom.x,
                                                newRandom.y].GetCanvasPosition());

                        arena[newRandom.x, newRandom.y].
                               ChangeArenaTileState(ArenaTileState.BLOCK,block);

                        return block.type;
                    }
                }
            }

            for (int x = 0; x < random.x - 1; x++)
            {
                for (int y = 0 + 1; y < random.y - 1; y++)
                {
                    newRandom.x = x;
                    newRandom.y = y;
                    if (arena[newRandom.x, newRandom.y].GetArenaTileState() == ArenaTileState.EMPTY)
                    {
                        var block = BlocksManager.EnableBlock(newRandom,arena[newRandom.x,
                                               newRandom.y].GetCanvasPosition());

                        arena[newRandom.x, newRandom.y].
                               ChangeArenaTileState(ArenaTileState.BLOCK, block);
                        return block.type;
                    }
                }
            }

            Debug.Log("No empty spaces");
            return BlockType.INACTIVE;
        }


        #endregion


        internal bool IsSnakeValidArenaTile(Position position)
        {
            return arena[position.x, position.y].GetArenaTileState() == ArenaTileState.EMPTY ||
                    arena[position.x, position.y].GetArenaTileState() == ArenaTileState.BLOCK;
        }
        
        public bool IsEmptyArenaTile(int x, int y)
        {
            return arena[x, y].GetArenaTileState() == ArenaTileState.EMPTY;
        }

        public ArenaTileState CheckTileForSnake(int x, int y, out BlockType blockType)
        {
            blockType = BlockType.INACTIVE;
            var result = arena[x, y].GetArenaTileState();

            if (result == ArenaTileState.BLOCK)
            {

                blockType = arena[x, y].block.type;

                //Desable eaten block
                BlocksManager.DesableBlock(arena[x,y].block);
                //Enable a new block
                SetBlock();

                arena[x, y].ChangeArenaTileState(ArenaTileState.SNAKE);
            }

            arena[x, y].ChangeArenaTileState(ArenaTileState.SNAKE);



            return result;
        }


        public void  SetArenaTileState(int x, int y, ArenaTileState arenaTileState)
        {
            arena[x,y].ChangeArenaTileState(arenaTileState);
        }


        public Position GetNearBlockPosition(Position position )
        {
            return BlocksManager.GetNearBlock(position);
        }

        public void ReturnTime()
        {
            for (int i = 0; i < snakes.Count; i++)
            {
                snakes[i].ReturnTime();
            }
        }


        #region GameOver
        public void GameOver()
        {
            ResetArena();
            for (int i = 0; i < snakeAmount; i++)
            {
                snakes[i].CreateSnake(gameData.initialSnakeSize,
                            gameData.arenaHeight, tileSize, gameData.snakeSpeed);
            }

        }

        void ResetArena()
        {
            for (int y = 0; y < gameData.arenaHeight; y++)
            {
                for (int x = 0; x < gameData.arenaWidth; x++)
                {
                    arena[x, y].ChangeArenaTileState(ArenaTileState.EMPTY);
                }
            }
        }
    }

    #endregion
}
#pragma warning restore CS0649