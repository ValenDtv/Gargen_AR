using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObjectCollector Collector;
    GameObject canvas;
    GameObject menu_canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = Collector.GameObjects.Canvas;
        menu_canvas = Collector.GameObjects.MenuCanvas;
    }


    public void OpenMenu()
    {
        menu_canvas.SetActive(true);
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
    }

    public void CloseMenu()
    {
        menu_canvas.SetActive(false);
        canvas.GetComponent<GraphicRaycaster>().enabled = true;
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
