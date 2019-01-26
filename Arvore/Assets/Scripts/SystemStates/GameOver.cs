using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Snake
{
    public class GameOver : MonoBehaviour
    {
        Image[] images;
        Text[] texts;

        //Is set in Game system 
        public Button button;

        private void Awake()
        {
            images = GetComponentsInChildren<Image>();
            texts = GetComponentsInChildren<Text>();
        }

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

        public void Enable(bool enable)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].enabled = enable;
            }

            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].enabled = enable;
            }
        }

    }
}
