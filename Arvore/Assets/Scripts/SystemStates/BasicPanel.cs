using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace Snake
{
    public class BasicPanel : MonoBehaviour
    { 
        [SerializeField] public Button changeStateButton;
        Image[] images;
        Text[] texts;

        public void SetPanel()
        {
            images = GetComponentsInChildren<Image>();
            texts = GetComponentsInChildren<Text>();

            Enable(false);
        }

        //Todo why this not work
        //public void SetChangeStateButton(UnityAction func)
        //{
        //    changeStateButton.onClick.AddListener(func);
        //}

        public void RemoveAllListeners()
        {
            changeStateButton.onClick.RemoveAllListeners();
        }

        //Desable component are more performatic than deactivate the entire 
        //game object
        public virtual void Enable(bool enable)
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
