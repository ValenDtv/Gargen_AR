using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;

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
    private string url = "http://127.0.0.1:8000/";

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

    public bool UpdateThirst(Plant plant)
    {
        ThirstUpdate data = new ThirstUpdate(Collector.Userid, plant.id, plant.thirst-1);
        string jsonData = JsonUtility.ToJson(data);
        using (var webClient = new WebClient())
        {
            
            string response = webClient.UploadString(url + "thirst/", jsonData);
            dynamic json = JValue.Parse(response);
            if (json.success.Value)
            {
                plant.thirst--;
                return true;
            }
        }
        return false;
    }
}
