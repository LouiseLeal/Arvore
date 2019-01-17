using System.Collections.Generic;
using UnityEngine;


namespace Snake
{
   
    public class Snake : MonoBehaviour
    {
        [SerializeField] private GameObject snakeTilePrefab;
        List<SnakeTile> snake;

        public void CreateSnake(int initialTileSize, int arenaHeight, Vector2 TileSize)
        {
            var newSnakeTile = (Instantiate(snakeTilePrefab, this.transform)
                                        as GameObject).GetComponent<SnakeTile>();


            //Define an rando y position of snake
            var snakeTilePositionY = Random.Range(1, arenaHeight - 1);
            //Consider not position on wall;
            var snakeTilePositionX =initialTileSize;
            var snakeTilePosition = new Vector2(snakeTilePositionX, snakeTilePositionY);


            newSnakeTile.SetSnake(snakeTilePosition, TileSize, true);

            for (int i = 1; i < initialTileSize; i++)
            {
                newSnakeTile = (Instantiate(snakeTilePrefab, this.transform)
                                        as GameObject).GetComponent<SnakeTile>();

                snakeTilePosition.x = snakeTilePositionX - i;

                newSnakeTile.SetSnake(snakeTilePosition, TileSize, false);
            }
        }
    }
}