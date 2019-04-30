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
    private bool emission = false;


    // Start is called before the first frame update
    void Start()
    {
        options.Collector.GetComponent<GameObjectCollector>().GameObjects.WateringCan_button
                                   .GetComponent<Button>().onClick.AddListener(ButtonIsCliked);
        wateringCan = options.Collector.GetComponent<GameObjectCollector>().GameObjects.WateringCan;
        StopEmission();
    }

    public void ButtonIsCliked()
    {
        emission = !emission;
        if (emission) StartEmission();
        else StopEmission();
    }

    public void StartEmission()
    {
        //this.gameObject.GetComponent<ParticleSystem>().Play();
        wateringCan.SetActive(true);
        //this.gameObject.GetComponent<ParticleSystem>().startLifetime = 5;
        Debug.Log("Частица появляются");
    }

    public void StopEmission()
    {
        wateringCan.SetActive(false);
        //this.gameObject.GetComponent<ParticleSystem>().Stop();
        //this.gameObject.GetComponent<ParticleSystem>().startLifetime = 0;
        Debug.Log("Частицы исчезают");
    }
}
