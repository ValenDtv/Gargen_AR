using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Plant_button_options
{
    public GameObject Collector;

}


public class Thing_button_script : MonoBehaviour
{

    public Plant_button_options options;
    private GameObject thingPanel;
    private GameObject grid;
    private GameObject confirm;

    // Start is called before the first frame update
    void Start()
    {
        grid = options.Collector.GetComponent<GameObjectCollector>().GameObjects.Grid;
        confirm = options.Collector.GetComponent<GameObjectCollector>().GameObjects.Confirm;
        thingPanel = options.Collector.GetComponent<GameObjectCollector>().GameObjects.Thing_panel;
    }

    public void OnMouseDown()
    {
        thingPanel.SetActive(true);
        //передать скрипту кнопи мод 
    }
}
