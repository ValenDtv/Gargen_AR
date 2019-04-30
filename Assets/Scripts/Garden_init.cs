using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Garden_options
{
    public GameObject Collector;

}

public class Garden_init : MonoBehaviour
{
    public Garden_options options;
    private GardenAR_db gardenAR_db;
    private Saver_script saver;

    // Start is called before the first frame update
    void Start()
    {
        gardenAR_db = options.Collector.GetComponent<GameObjectCollector>().gardenAR_db;
        saver = options.Collector.GetComponent<GameObjectCollector>().GameObjects.Saver.GetComponent<Saver_script>();
        foreach (Plant plant in gardenAR_db.plants)
        {
            System.DateTime create = System.DateTime.Parse(plant.create);
            System.DateTime update = System.DateTime.Parse(plant.update);
            int HoursPassed = (System.DateTime.Now - create).Hours - (update - create).Hours;
            for (int i=3; i <= HoursPassed; i+=3)
            {
                int points_added = 125;
                if (plant.thirst < 5) plant.thirst++;
                if (plant.bugs < 5)
                {
                    float chance = Random.Range(0f, 1f);
                    if ( chance < 0.2f)
                        plant.bugs++;
                }
                points_added -= 12 * plant.thirst + 12 * plant.bugs;
                plant.points += points_added;
            }
            for (int i=0; i < 6; i++)
            {
                if (plant.points > i*plant.max_points/6)
                    plant.stage = i+1;
            }
            plant.update = System.DateTime.Now.ToString();
        }
        saver.Save_DB();
    }

}
