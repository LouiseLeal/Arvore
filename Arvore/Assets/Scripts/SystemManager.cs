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

        private void Awake()
        {

            ChangeState(GameState.START);
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
            gameState = GameState.START;
            startGame.Enable(true);
            startGame.button.onClick.AddListener(() => {

                ChangeState(GameState.GAME);
                startGame.Enable(false);
                startGame.button.onClick.RemoveAllListeners();

            });

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