using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Snake
{
    enum GameState
    {
        START,
        GAME,
        GAME_OVER,
        MENU
    }

    public class SystemManager : Singleton<SystemManager>
    {
        GameState gameState;

        [SerializeField] StartGame startGame;
        [SerializeField] GameOver gameOver;

        [SerializeField] GameData gameData;
        [SerializeField] Canvas canvas;

        private void Awake()
        {
            if (gameData == null)
                gameData = Resources.Load<GameData>("GameDataDefault");

            if (canvas == null)
                canvas = transform.parent.GetComponent<Canvas>();

            ChangeState(GameState.START);

            //Desable othes panels
            gameOver.SetPanel();
            gameOver.Enable(false);
        }

        void ChangeState(GameState state)
        {
            switch (state)
            {
                case GameState.START:
                    GameSateStart();
                    break;
                case GameState.GAME:
                    GameManager.Instance.PreGame();
                    break;
                case GameState.GAME_OVER:
                    break;
                case GameState.MENU:
                    break;
            }
        }
       

        void GameSateStart()
        {
            Vector2 tileSize = CalculeteTileSize();
            gameState = GameState.START;
            startGame.Enable(true);

            startGame.button.onClick.AddListener(() => {

                ChangeState(GameState.GAME);
                startGame.Enable(false);
                startGame.button.onClick.RemoveAllListeners();

            });

        }

        private Vector2 CalculeteTileSize()
        {
            Vector2 tileSize = new Vector2(0, 0);


            var rect = canvas.pixelRect;
            Debug.Log("rect x " + rect.width + " rect y " + rect.height);

            var width =  rect.width / gameData.arenaWidth;
            var height = rect.height /gameData.arenaHeight;

            //Calculate the tile size with the smaller proportion
           
            if (width < height) 
                //find hight gap to centralize arena
            {
                float offset = rect.height - (width * gameData.arenaHeight);
                gameData.tileSize = width;
                gameData.tileOffSet = new Vector2(-width/2f , offset/2f);
            }
            else
            {

                float offset = rect.width - (height * gameData.arenaWidth);
                gameData.tileSize = height;
                gameData.tileOffSet = new Vector2((offset/2f), - height/2f);
            }

            return tileSize;
        }

        public void GameOver(Snake snake)
        {
            gameState = GameState.GAME_OVER;
            gameOver.StartGameOver(snake);

            gameOver.button.onClick.AddListener(() => {

                ChangeState(GameState.START);
                gameOver.Enable(false);
                gameOver.button.onClick.RemoveAllListeners();
            });

        }
    }
}