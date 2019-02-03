using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private GameObject mapTilePrefab;
        [SerializeField] private BlocksSpawnManager blocksManager;
        [SerializeField] private DefineInput defineInput;

        ArenaTile[,] arena;

        [HideInInspector] public Vector2 tileSize;

        [SerializeField] Cannon cannon;

        [Header("Snakes Settings")]
        [SerializeField] private SnakePlayer snakePlayerPrefab;
        [SerializeField] private SnakeAI snakeAIPrefab;
        List<Snake> snakes;
        int snakeAITotal = 0;
        int snakePlayerTotal = 0;

        [Header("Containers")]

        [SerializeField] private RectTransform arenaContainer;
        [SerializeField] private RectTransform snakeContainer;
        [SerializeField] private RectTransform blockContainer;
        [SerializeField] private RectTransform cannonContaine;

        bool gameStarted = false;

        //OBS : The number maximum of selected snakes will be:
        // the number of tiles - 3 (2 becaus of the wall and another one for space) 
        //devide by 2(half of player snakes and half for AI snakes). 
        int MaxSnakes = 0;

        #region Unity Methods
        public void Awake()
        {
            //Todo Is Needed ??
            if (gameData == null)
                gameData = Resources.Load<GameData>("GameDataDefault");
        }

        //Check if a snake dies 
        // Makes it not evaluate in the same frame as the snakes dies
        private void Update()
        { //Todo why this not work
            if (gameStarted)
                CheckForGameOver();
        }

        #endregion

        public void PreGame()
        {
            gameStarted = false;

            //Get already setted data to set this game
            tileSize = new Vector2(gameData.tileSize, gameData.tileSize);

            CreateArena();
            SetContainersPosition();

            snakePlayerTotal = 0;
            snakeAITotal = 0;

            snakes = new List<Snake>();
            MaxSnakes = (gameData.arenaHeight - 3) / 2;

            defineInput.StartCheckingInput();

            defineInput.RemoveAllEvents();

            defineInput.CreatSnake += CreatePlayerSnake;
            defineInput.DefinedOneInput += SelectedSnakePreset;
            defineInput.finishDefineInput += StartGame;


        }

        public void StartGame()
        {

            Debug.Log("StartGame");
            //snakeAmount = gameData.snakesPlayerAmount;
            
            tileSize = new Vector2(gameData.tileSize, gameData.tileSize);



            blocksManager.SetBlockChances(gameData.grayBlock, gameData.greenBlock,
                                            gameData.blueBlock, gameData.redBlock);


            blocksManager.CreateBlockPool(5, tileSize);

            SetGame();

            gameStarted = true;
        }
       

        void SetContainersPosition()
        {

            arenaContainer.anchoredPosition = gameData.tileOffSet;
            snakeContainer.anchoredPosition = gameData.tileOffSet;
            blockContainer.anchoredPosition = gameData.tileOffSet;
            cannonContaine.anchoredPosition = gameData.tileOffSet;
        }

        //Todo change it for use on only ai snakes
        public void CreateAISnake()
        {
            var newSnake = Instantiate(snakeAIPrefab, snakeContainer).GetComponent<Snake>();
            newSnake.CreateSnake(gameData.initialSnakeSize, gameData.arenaHeight, tileSize, gameData.snakeSpeed);

            snakes.Add(newSnake);
            snakeAITotal++;
        }

        // Instatiate players snakes
        public void CreatePlayerSnake(KeyCode[] input)
        {
            Debug.Log(" snakePlayer " + snakePlayerTotal + " " + MaxSnakes);
            if (snakePlayerTotal >= MaxSnakes)
                return;

            var newSnake = Instantiate(snakePlayerPrefab, snakeContainer).GetComponent<SnakePlayer>();
            newSnake.SetInput(input);
            newSnake.CreateSnake(gameData.initialSnakeSize, gameData.arenaHeight, tileSize, gameData.snakeSpeed);
            snakes.Add(newSnake);


            snakePlayerTotal++;
        }

        public void SelectedSnakePreset()
        {
            snakes[snakes.Count - 1].SelectSnakePreset();
        }

        void SetGame()
        {
            Debug.Log("SetGame");
            blocksManager.ResetBlocks();
            SetBlock();

            cannon.SetTileSize(tileSize);

            for (int i = 0; i < snakePlayerTotal; i++)
            {
                snakes[i].SetActive(true);
                //Create an AI snake for every player snake
                CreateAISnake();
            }

            snakes[0].ResetUsedColors();
        }

        #region GameSetting

        //Create arena programaticaly
        private void CreateArena()
        {
         
            //set variables before for the avoid garbage collection
            GameObject newGameObject;
            ArenaTile newTile;
            Vector2 newCanvasPosition = Vector2.zero;

            arena = new ArenaTile[gameData.arenaWidth, gameData.arenaHeight];

            for (int y = 0; y < gameData.arenaHeight; y++)
            {
                for (int x = 0; x < gameData.arenaWidth; x++)
                {
                    newGameObject = Instantiate(mapTilePrefab, arenaContainer) as GameObject;

                    //Use string build for more optimized code
                    StringBuilder tileName = new StringBuilder("arena ", 30);
                    tileName.Append(x.ToString());
                    tileName.Append(" ");
                    tileName.Append(y.ToString());
                    newGameObject.name = tileName.ToString();

                    newTile = newGameObject.GetComponent<ArenaTile>();
                    newTile.SetTileSize(tileSize);

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
                if (IsEmptyArenaTile(position.x, i))
                {
                    position.y = i;
                    return position;
                }
            }

            //Check for smaller y empty value

            for (int i = 0; i < position.y - 1; i++)
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

        public Vector2 GetCanvasPosition(int x, int y)
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

                Block block = blocksManager.EnableBlock(random, arena[random.x,
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

                        var block = blocksManager.EnableBlock(newRandom, arena[newRandom.x,
                                                newRandom.y].GetCanvasPosition());

                        arena[newRandom.x, newRandom.y].
                               ChangeArenaTileState(ArenaTileState.BLOCK, block);

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
                        var block = blocksManager.EnableBlock(newRandom, arena[newRandom.x,
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
                blocksManager.DesableBlock(arena[x, y].block);
                //Enable a new block
                SetBlock();

                arena[x, y].ChangeArenaTileState(ArenaTileState.SNAKE);
            }

            arena[x, y].ChangeArenaTileState(ArenaTileState.SNAKE);



            return result;
        }


        public void SetArenaTileState(int x, int y, ArenaTileState arenaTileState)
        {
            arena[x, y].ChangeArenaTileState(arenaTileState);
        }


        public Position GetNearBlockPosition(Position position)
        {
            return blocksManager.GetNearBlock(position);
        }


        public Vector2 GetArenaCanvasPosition(Position position1)
        {
            return arena[position1.x, position1.y].GetCanvasPosition();
        }


        #region BlocksPowers
        public void ReturnTime()
        {
            for (int i = 0; i < snakes.Count; i++)
            {
                snakes[i].ReturnTime();
            }
        }

        public void FireBullet(Snake snake)
        {
            var nearSnake =  GetNearSnakeHead(snake, gameData.rangeConnon);
            cannon.ShootCannon(snake,nearSnake, gameData.cannonThreshold, 
                                                        gameData.cannonSpeed);
        }

        private Snake GetNearSnakeHead(Snake snake, int rangeConnon)
        {
            List<Snake> inRangeSnakes = new List<Snake>();
            //The range is already taking out the arena bounds
            for (int i = 0; i < snakes.Count; i++)
            {
                if (snake == snakes[i])
                    continue;

                if (snake.IsOtherSnakeInRange(snakes[i], rangeConnon) == null)
                    inRangeSnakes.Add(snakes[i]);
                
            }
            if (inRangeSnakes.Count == 0)
                return null;

            int rand = UnityEngine.Random.Range(0, inRangeSnakes.Count - 1);

            return inRangeSnakes[rand];

        }

        #endregion

        //Methods for check and run game over
        #region GameOver
        public void SnakeDie(Snake snake)
        {
            Debug.Log("snake die" + name);  
            if (!snakes.Remove(snake))
                Debug.LogError("Error when snake AI die");

            if (snake.IsSnakeAI())
            {
                snakeAITotal--;
            }
            else
            {
                snakePlayerTotal--;
            }

            snake.Die();

        }

        bool CanFastFoward = false;


        void CheckForGameOver()
        {

            if (snakes.Count == 1)
            {
                SetGameOver(snakes[0]);
                snakes.Remove(snakes[0]);
            }
            else if (snakePlayerTotal > 1) { 
                CanFastFoward = true;
            }
        }

        void SetGameOver(Snake snake)
        {
            SystemManager.Instance.GameOver(snake);
            snake.isActive = false;

            blocksManager.ResetBlocks();
            ResetArena();
            snakeAITotal = 0;
            snakePlayerTotal = 0;
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