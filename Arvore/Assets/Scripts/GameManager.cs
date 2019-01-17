
using UnityEngine;

namespace Snake
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameData gameData;
        [SerializeField] private GameObject MapTilePrefab;
        [SerializeField] private Transform ArenaContainer;


        ArenaTile[,] arena;


        private void Awake()
        {
            if (gameData == null)
                gameData = Resources.Load<GameData>("GameDataDefault");

            arena = new ArenaTile[gameData.arenaWidth, gameData.arenaHeight];
        }

        private void Start()
        {
            CreateArena();

        }

        private void CreateArena()
        {
            GameObject newGameObject;
            ArenaTile newTile;

            //Todo put it on game config
            Vector2 sizeDelta = new Vector2(50 , 50);

            for (int i = 0; i < gameData.arenaWidth; i++)
            {
                for (int j = 0; j < gameData.arenaHeight; j++)
                {
                    newGameObject = Instantiate(MapTilePrefab, ArenaContainer) as GameObject;
                    newTile = newGameObject.GetComponent<ArenaTile>();
                    newTile.rectTransform.sizeDelta = sizeDelta;
                    newTile.rectTransform.anchoredPosition = new Vector2((sizeDelta.x * i), (sizeDelta.y * j));

                    if (i == 0 || i == gameData.arenaWidth - 1 || j == 0 || j == gameData.arenaHeight - 1)
                        newTile.SetWall();

                    arena[i, j] = newTile;
                }
            }
        }
    }


}