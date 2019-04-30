using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class Saver_options
{
    public GameObject Collector;

}

public class Saver_script : MonoBehaviour
{
    public Saver_options options;
    private GardenAR_db gardenAR_db;
    private GameObjectCollector Collector;
    private string path;

    // Start is called before the first frame update
    void Start()
    {
        Collector = options.Collector.GetComponent<GameObjectCollector>();
        gardenAR_db = Collector.gardenAR_db;
        path = Application.persistentDataPath + @"/GardenAR_db.json";
    }

    public void Save_DB()
    {
        string json = JsonUtility.ToJson(gardenAR_db);
        using (StreamWriter sr = new StreamWriter(path))
        {
            sr.Write(json);
        }
        //Collector.Load_DB();
    }
}
