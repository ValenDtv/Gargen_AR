using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class WateringCan_options
{
    public GameObject Collector;
}

public class WateringCan_script : MonoBehaviour
{
    public WateringCan_options options;
    private GameObject wateringCan;
    Image wateringCanButtonImage;
    private bool emission = false;

    // Start is called before the first frame update
    void Start()
    {
        options.Collector.GetComponent<GameObjectCollector>().GameObjects.WateringCan_button
                                   .GetComponent<Button>().onClick.AddListener(ButtonIsCliked);
        wateringCanButtonImage =
        options.Collector.GetComponent<GameObjectCollector>().GameObjects.WateringCan_button.GetComponent<Image>();
        wateringCan = options.Collector.GetComponent<GameObjectCollector>().GameObjects.WateringCan;
        StopEmission();
    }

    public void ButtonIsCliked()
    {
        emission = !emission;
        if (emission)
        {
            wateringCanButtonImage.color = new Color(0.387f, 0.922f, 0.996f);
            StartEmission();
        }
        else
        {
            wateringCanButtonImage.color = new Color(1f, 1f, 1f);
            StopEmission();
        }
    }

    public void StartEmission()
    {
        wateringCan.SetActive(true);
    }

    public void StopEmission()
    {
        wateringCan.SetActive(false);
    }
}
