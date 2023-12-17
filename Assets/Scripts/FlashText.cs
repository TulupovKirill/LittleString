using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlashText : MonoBehaviour
{
    public Text text;
    float transition, textTransparence;
    bool invisibility = true;

    void Start()
    {
        textTransparence = text.color.a * 255;
        //transition показывает за сколько времени текст будет исчезнуть а потом появляться.
        transition = 3.0f;
    }

    private void Update()
    {
        if (invisibility)
        {
            textTransparence -= Time.deltaTime * (255.0f / transition * 2);
            if (textTransparence <= 0.0f)
            {
                invisibility = false;
            }
        }
        else if (!invisibility)
        {
            textTransparence += Time.deltaTime * (255.0f / transition * 2);
            if (textTransparence >= 255.0f)
            {
                invisibility = true;
            }
        }
        text.color = new Color32((byte)text.color.r, (byte)text.color.g, (byte)text.color.b, (byte)textTransparence);
    }
}
