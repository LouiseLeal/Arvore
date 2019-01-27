using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Snake
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] RectTransform rect;
        [SerializeField] Image image;
        float threshold;
        float speed;

        public void SetTileSize(Vector2 TileSize)
        {
            rect.sizeDelta = TileSize;
        }

        public void ShootCannon(Snake snake, Snake nearSnake, float threshold, float speed)
        {
            this.speed = speed;
            this.threshold = threshold;
            StartCoroutine(TravelCannonCoroutine(snake, nearSnake));
        }

        private IEnumerator TravelCannonCoroutine(Snake snake, Snake nearSnake)
        {
            var position1 = snake.GetHeadPosition();
            var position2 = nearSnake.GetHeadPosition();

            Vector2 p1 = GameManager.Instance.GetArenaCanvasPosition(position1);
            Vector2 p2 = GameManager.Instance.GetArenaCanvasPosition(position1);

            rect.anchoredPosition = p1;
            image.enabled = true;

            while (Vector2.Distance(rect.anchoredPosition, p2) > threshold)
            {
                rect.anchoredPosition = Vector2.Lerp(p1, p2, speed);
                yield return new WaitForEndOfFrame();
            }

            image.enabled = false;
            rect.anchoredPosition = Vector2.zero;

            GameManager.Instance.SnakeDie(nearSnake);
        }
    }
}
