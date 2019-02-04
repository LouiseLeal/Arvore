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
        DEFINE_INPUT,
        GAME,
        GAME_OVER,
        MENU
    }

    public class SystemManager : Singleton<SystemManager>
    {
        GameState gameState;

        [SerializeField] StartGame startGamePanel;
        [SerializeField] BasicPanel defineSnakes;
        [SerializeField] GameOver gameOverPanel;

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
            gameOverPanel.SetPanel();
            defineSnakes.SetPanel();

        }

        void ChangeState(GameState state)
        {
            switch (state)
            {
                case GameState.START:
                    GameSateStart();
                    break;
                case GameState.DEFINE_INPUT:
                    DefineSnakes();
                    break;
                case GameState.GAME:
                    GameManager.Instance.StartGame();
                    break;
                case GameState.GAME_OVER:
                    break;
                case GameState.MENU:
                    break;
                
            }
        }


        void GameSateStart()
        {
            if (!startGamePanel.gameObject.activeSelf)
                startGamePanel.gameObject.SetActive(true);

            Vector2 tileSize = CalculeteTileSize();
            gameState = GameState.START;
            startGamePanel.Enable(true);

            startGamePanel.changeStateButton.onClick.AddListener(() => {
                Debug.Log("StartStat");
                ChangeState(GameState.DEFINE_INPUT);
                startGamePanel.Enable(false);
                startGamePanel.changeStateButton.onClick.RemoveAllListeners();

            });
        }

        public void DefineSnakes()
        {
            if (!defineSnakes.gameObject.activeSelf)
                defineSnakes.gameObject.SetActive(true); 

            gameState = GameState.DEFINE_INPUT;
            defineSnakes.Enable(true);

            GameManager.Instance.PreGame();

            //Todo see if this can be simplified
            defineSnakes.changeStateButton.onClick.AddListener(() => {
                if (GameManager.Instance.HasAnySnake())
                {
                    ChangeState(GameState.GAME);
                    defineSnakes.Enable(false);
                    defineSnakes.changeStateButton.onClick.RemoveAllListeners();
                }
            });

        }


        public void GameOver(Snake snake)
        {
            if (!gameOverPanel.gameObject.activeSelf)
                gameOverPanel.gameObject.SetActive(true);

            gameState = GameState.GAME_OVER;
            gameOverPanel.StartGameOver(snake);

            gameOverPanel.changeStateButton.onClick.AddListener(() => {

                ChangeState(GameState.START);
                gameOverPanel.Enable(false);
                gameOverPanel.changeStateButton.onClick.RemoveAllListeners();
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
    }
}