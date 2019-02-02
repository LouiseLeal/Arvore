using UnityEngine;
using UnityEngine.UI;


namespace Snake
{
    public class StartGame : MonoBehaviour
    {
        [SerializeField] private Image panelImage;
        [SerializeField] public Button changeStateButton;

        Image[] images;
        Text[] texts;

        private void Awake()
        {
            images = GetComponentsInChildren<Image>();
            texts = GetComponentsInChildren<Text>();
        }
       
        public  void Enable(bool enable)
        {

            if (images != null)
            {
                for (int i = 0; i < images.Length; i++)
                {
                    images[i].enabled = enable;
                }
            }

            if (texts != null)
            {
                for (int i = 0; i < texts.Length; i++)
                {
                    texts[i].enabled = enable;
                }
            }
        }
    }
}
