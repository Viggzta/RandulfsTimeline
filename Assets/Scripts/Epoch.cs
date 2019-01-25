using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Epoch : MonoBehaviour
{
    public Image head;
    public Image body;
    public Image tail;

    public void SetColor(Color color)
    {
        head.color = color;
        body.color = color;
        tail.color = color;
    }
}
