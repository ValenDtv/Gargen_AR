using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VegetUnit_script : MonoBehaviour
{
    public GameObjectCollector Collector;
    private int num;
    private Saver_script saver;
    private Plant info;
    private int hits;
    public GameObject thirst;
    public GameObject bugs;
    private GameObject thingPanel;
    public Veget_script parent;
    private float start_Collision;
    private float end_Collision;
    private Collider cloud;
    private bool insecticides = false;
    private float partical_time = 0;
    private float max_partical_time = 0.5f;
    private float watering_time = 0;
    private bool is_watering = false;
    private TrackableBehaviour trackableBehaviour;


    // Start is called before the first frame update
    void Start()
    {
        saver = Collector.GameObjects.Saver.GetComponent<Saver_script>();
        parent = Collector.GameObjects.Veget.GetComponent<Veget_script>();
        thingPanel = Collector.GameObjects.Thing_panel;
        trackableBehaviour = Collector.GameObjects.ImageTarget.GetComponent<TrackableBehaviour>();
        num = System.Convert.ToInt32(this.gameObject.name);
        info = Collector.gardenAR_db.plants[num];
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Частицы попали");
        //if (info.thirst == 0)
        //    return;
        //hits++;
        ////Debug.Log(hits);
        //if (hits>80)
        //{
        //    info.thirst--;
        //    parent.WaterDropsInit(num);
        //    hits = 0;
        //}
        partical_time = 0;
        is_watering = true;
    }

    private void OnMouseDown()
    {
        if (trackableBehaviour.CurrentStatus != TrackableBehaviour.Status.TRACKED || thingPanel.activeSelf)
            return;
        if (info.stage != 6)
            return;
        bool flag = true;
        Destroy(this.gameObject.transform.Find("tree/Fetus").gameObject);
        foreach (Stock fetus in Collector.gardenAR_db.stock)
            if (fetus.name == info.fetus)
            {
                fetus.count += 2;
                flag = false;
            }
        if (flag)
            Collector.gardenAR_db.stock.Add(new Stock(info.fetus, info.file, 2));
        info.stage = 5;
        info.points = 5000;
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Попал в дерево");
        start_Collision =  Time.time;
        cloud = other;
        insecticides = true;
    }

    private void OnTriggerExit(Collider other)
    {
        InsecticideResult();
    }

    private void InsecticideResult()
    {
        end_Collision = Time.time;
        float seconds_passed = end_Collision - start_Collision;
        if (seconds_passed >= 1.5f)
            info.bugs--;
        if (seconds_passed >= 1f)
        {
            info.bugs--;
            parent.BugsInit(num);
        }
        insecticides = false;
        //Debug.Log("Секунд прошло: " + seconds_passed.ToString());
    }


    private void Update()
    {
        if (is_watering && info.thirst > 0)
        {
            partical_time += Time.deltaTime;
            if (partical_time >= max_partical_time)
            {
                watering_time = 0;
                is_watering = false;
            }
            else
            {
                watering_time += Time.deltaTime;
                if (watering_time >= 2f)
                {
                    info.thirst--;
                    parent.WaterDropsInit(num);
                    watering_time = 0;
                }
            }
        }
        if (insecticides && info.bugs > 0)
            if (cloud == null)
                InsecticideResult();
    }
}
