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

        private void Awake()
        {
          
            GameSateStart();
        }


        void GameSateStart()
        {
            gameState = GameState.START;
            startGame.Enable(true);
            startGame.button.onClick.AddListener(() => {

                ChangeState(GameState.GAME);
                startGame.Enable(false);

            });

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
    }
}