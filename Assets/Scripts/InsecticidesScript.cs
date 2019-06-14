using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Insecticides_options
{
    public GameObject Collector;

}

public class InsecticidesScript : MonoBehaviour
{
    public Insecticides_options options;
    private GameObject cloud;
    private bool is_active = false;
    private GameObject ImageTarget;
    private GameObject ARcamera;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        options.Collector.GetComponent<GameObjectCollector>().GameObjects.Insecticides_button
                                   .GetComponent<Button>().onClick.AddListener(CreateCloud);
        ImageTarget = options.Collector.GetComponent<GameObjectCollector>().GameObjects.ImageTarget;
        ARcamera = options.Collector.GetComponent<GameObjectCollector>().GameObjects.AR_camera;
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_active)
            return;
        if (time <= 0 || cloud.transform.localPosition.y <= 0)
        {
            is_active = false;
            Destroy(cloud);
            return;
        }
        cloud.transform.Translate(Vector3.forward * 5 * Time.deltaTime);
        time -= Time.deltaTime;
    }

    public void CreateCloud ()
    {
        if (is_active)
            return;
        cloud = Instantiate(Resources.Load<GameObject>("InsecticideCloud"));
        cloud.transform.SetParent(ImageTarget.transform);
        cloud.transform.localPosition = new Vector3(0, 0, 0);
        cloud.transform.position = ARcamera.transform.position;
        
        is_active = true;
        time = 5;
    }
}
