using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventConstraints : MonoBehaviour
{
    public int maxWidth = 325;
    public Text textComponentTitle;
    public Text textComponentBody;
    public Text textComponentTime;
    public Font font;
    public GameObject window;

    private void AutoResizeWindow()
    {
        CharacterInfo info;
        int totalWidth = 0;
        font.RequestCharactersInTexture(textComponentBody.text);

        for (int i = 0; i < textComponentBody.text.Length; i++)
        {
            font.GetCharacterInfo(textComponentBody.text[i], out info);
            totalWidth += info.glyphWidth;
        }

        RectTransform windowTransform = window.GetComponent<RectTransform>();

        if (totalWidth > maxWidth)
        {
            int height = (totalWidth / maxWidth) + 1;

            windowTransform.sizeDelta = new Vector2(maxWidth, height * (font.fontSize + 5) + 46);
        }
        else
        {
            windowTransform.sizeDelta = new Vector2(totalWidth, (font.fontSize + 5) + 46);
        }

        MoveIntoFrame();
    }

    public void MoveIntoFrame()
    {
        RectTransform windowTransform = window.GetComponent<RectTransform>();
        if (windowTransform.position.x - windowTransform.sizeDelta.x < 25)
        {
            windowTransform.position += new Vector3(windowTransform.sizeDelta.x / 2 - 25, 0, 0);
        }

        if ((windowTransform.position.x + windowTransform.sizeDelta.x) > (Screen.width + 25))
        {
            windowTransform.position -= new Vector3(windowTransform.sizeDelta.x / 2 - 25, 0, 0);
        }
    }

    public void SetText(string title, string body, int hours, int minutes, float seconds)
    {
        textComponentTitle.text = title;
        textComponentBody.text = body;
        textComponentTime.text = hours + ":" + minutes + ":" + seconds;
        AutoResizeWindow();
    }
}
