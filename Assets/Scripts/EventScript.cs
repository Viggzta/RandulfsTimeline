using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventScript : MonoBehaviour
{
    public GameObject window;
    private bool keepActive = false;
    private Button selfButton;

    private void Start()
    {
        selfButton = GetComponent<Button>();
    }

    public void OnPointerEnter()
    {
        window.SetActive(true);
    }

    public void OnPointerExit()
    {
        if (keepActive == false)
        {
            window.SetActive(false);
        }
    }

    public void OnPressed()
    {
        keepActive = !keepActive;
        if (keepActive == false)
        {
            selfButton.image.color = Color.white;
        }
        else
        {
            selfButton.image.color = Color.gray;
        }
    }
}
