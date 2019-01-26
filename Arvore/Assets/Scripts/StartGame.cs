using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField] public Button button;
    [SerializeField] public Image startPanel;
    Image[] images;
    Text[] texts;

    private void Awake()
    {
        images =GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<Text>();
    }

    public void Enable(bool enable)
    {
        startPanel.enabled = enable;
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
