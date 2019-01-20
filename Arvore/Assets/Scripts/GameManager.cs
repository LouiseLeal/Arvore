using UnityEngine;

namespace Snake
{

    public struct Posiiton
    {
        int x;
        int y;

    }

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] public GameData gameData;
        [SerializeField] private GameObject MapTilePrefab;
        [SerializeField] private Transform ArenaContainer;
        [SerializeField] private BlocksSpawnManager BlocksManager;

        [SerializeField] private Snake snake;

        ArenaTile[,] arena;

        Vector2 tileSize;


        #region UnityMethods
        private void Awake()
        {
            //Todo create an method to calculate it
            tileSize = new Vector2(gameData.TileSize, gameData.TileSize);

            if (gameData == null)
                gameData = Resources.Load<GameData>("GameDataDefault");

            arena = new ArenaTile[gameData.arenaWidth, gameData.arenaHeight];

            BlocksManager.SetBlockChances(gameData.grayBlock, gameData.greenBlock,
                                            gameData.blueBlock, gameData.redBlock);


            BlocksManager.CreateBlockPool(5, tileSize);


        }

        private void Start()
        {
            CreateArena();

            SetBlock();
            snake.CreateSnake(gameData.initialSnakeSize, gameData.arenaHeight, tileSize, gameData.snakeSpeed);
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

                    //Todo use string builder
                    newGameObject.name = "arena " + x + " " + y;

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

        public Vector2 GetCanvasPosition(int x,int y)
        {
            return arena[x, y].GetCanvasPosition();
        }

        //Todo find a better way
        private void SetBlock()
        {
            int randomX = Random.Range(1, gameData.arenaWidth - 2);
            int randomY = Random.Range(1, gameData.arenaHeight - 2);

            if (arena[randomX, randomY].GetArenaTileState() == ArenaTileState.EMPTY)
            {
                BlocksManager.EnableBlock(arena[randomX, randomY].GetCanvasPosition());
                return;
            }

            var newRandomX = randomX + 1;
            var newRandomY = randomY + 1;
            for (int x = newRandomX; x < gameData.arenaWidth - 1; x++)
            {
                for (int y = newRandomY; y < gameData.arenaHeight - 1; y++)
                {
                    newRandomX = x;
                    newRandomY = y;
                    if (arena[newRandomX, newRandomY].GetArenaTileState() == ArenaTileState.EMPTY)
                    {
                        BlocksManager.EnableBlock(arena[newRandomX, newRandomY].GetCanvasPosition());
                        return;
                    }
                }
            }

            for (int x = 0; x < randomX - 1; x++)
            {
                for (int y = 0 + 1; y < randomY - 1; y++)
                {
                    newRandomX = x;
                    newRandomY = y;
                    if (arena[newRandomX, newRandomY].GetArenaTileState() == ArenaTileState.EMPTY)
                    {
                        BlocksManager.EnableBlock(arena[newRandomX, newRandomY].GetCanvasPosition());
                        return;
                    }
                }
            }

            Debug.Log("No empty spaces");
        }

        #endregion


        public void  SetArenaTileState(int x, int y, ArenaTileState arenaTileState)
        {
            arena[x, y].ChangeArenaTileState(arenaTileState);
        }


        #region GameOver
        public void GameOver()
        {
            ResetArena();
            snake.CreateSnake(gameData.initialSnakeSize, gameData.arenaHeight, tileSize, gameData.snakeSpeed);
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