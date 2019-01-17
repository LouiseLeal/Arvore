
using System;
using UnityEngine;

namespace Snake
{

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private GameData gameData;
        [SerializeField] private GameObject MapTilePrefab;
        [SerializeField] private Transform ArenaContainer;

        [SerializeField] private Snake snake;


        ArenaTile[,] arena;


        private void Awake()
        {
            if (gameData == null)
                gameData = Resources.Load<GameData>("GameDataDefault");

            arena = new ArenaTile[gameData.arenaWidth, gameData.arenaHeight];
        }

        private void Start()
        {
            //Todo create an method to calculate it
            var tileSize = new Vector2(50, 50); 
            CreateArena(tileSize);
            snake.CreateSnake(gameData.initialSnakeSize, gameData.arenaHeight, tileSize);
        }

        private void CreateArena(Vector2 tileSize)
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

    }


}