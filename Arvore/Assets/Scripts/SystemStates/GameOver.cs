using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Snake
{
    public class GameOver : BasicPanel
    {
        public void StartGameOver(Snake snake)
        {
            Enable(true);

            if (snake.IsSnakeAI())
            {
                Debug.Log("Game over " + snake.name);
            }
            else
            {
                Debug.Log("Victory " + snake.name);
            }

            snake.Die();
        }
    }
}
