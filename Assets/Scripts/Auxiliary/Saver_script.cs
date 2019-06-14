using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using UnityEngine.SceneManagement;

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
    private Veget_script veget;
    private string path;
    System.DateTime lastupdate;
    //private string URL = "http://127.0.0.1:8000/";
    private string URL = "http://194.113.104.140:8000/";

    // Start is called before the first frame update
    void Start()
    {
        Collector = options.Collector.GetComponent<GameObjectCollector>();
        gardenAR_db = Collector.gardenAR_db;
        veget = Collector.GameObjects.Veget.GetComponent<Veget_script>();
        path = Application.persistentDataPath + @"/GardenAR_db.json";
        lastupdate = System.DateTime.Now;
    }

    void Update()
    {
        if ((System.DateTime.Now - lastupdate).Minutes > 5)
        {
            UpdatePlants();
            lastupdate = System.DateTime.Now;
        }
    }

    private void UpdatePlants()
    {
        string data = "userid=" + Collector.Userid.ToString();
        using (var webClient = new WebClient())
        {
            // Выполняем запрос по адресу и получаем ответ в виде строки
            string response = webClient.DownloadString(URL + "update_garden/" + "?" + data);
            dynamic json = JObject.Parse(response);
            if (json.success.Value)
            {
                int count = ((JArray)json["plants"]).Count;
                for (int i=0; i<count; i++)
                {
                    Plant renewed_plant = json["plants"][i].ToObject<Plant>();
                    for (int j=0; j<gardenAR_db.plants.Count; j++)
                    {
                        if (gardenAR_db.plants[j].id == renewed_plant.id)
                        {
                            if (gardenAR_db.plants[j].thirst != renewed_plant.thirst)
                            {
                                gardenAR_db.plants[j].thirst = renewed_plant.thirst;
                                veget.WaterDropsInit(j);

                            }
                            if (gardenAR_db.plants[j].bugs != renewed_plant.bugs)
                            {
                                gardenAR_db.plants[j].bugs = renewed_plant.bugs;
                                veget.WaterDropsInit(j);
                            }
                            gardenAR_db.plants[j].points = renewed_plant.points;
                            gardenAR_db.plants[j].stage = renewed_plant.stage;
                        }
                    }
                }
            }
        }
    }


    public void Save_DB()
    {
        string json = JsonUtility.ToJson(gardenAR_db);
        using (StreamWriter sr = new StreamWriter(path))
        {
            sr.Write(json);
        }
    }

    private string SendRequest(string url, string message, bool authentication)
    {
        using (var webClient = new WebClient())
        {
            if (authentication)
                webClient.Headers.Add(HttpRequestHeader.Authorization, Collector.Token);
            string response = webClient.UploadString(url, message);
            return response;
        }

    }

    private bool Authentication(bool result)
    {
        if (result)
            return true;
        else
        {
            SceneManager.LoadScene("LoginScene");
            return false;
        }
    }

    public int Save(ThirstUpdate data)
    {
        string jsonData = JsonUtility.ToJson(data);
        string response = SendRequest(URL + "thirst/", jsonData, true);
        dynamic json = JValue.Parse(response);
        if (!Authentication(json.authentication.Value))
            return -1;
        if (json.success.Value)
        {
            return  (int)json.thirst.Value;
        }
        return -1;
    }

    public int Save(BugsUpdate data)
    {
        string jsonData = JsonUtility.ToJson(data);
        string response = SendRequest(URL + "bugs/", jsonData, true);
        dynamic json = JValue.Parse(response);
        if (!Authentication(json.authentication.Value))
            return -1;
        if (json.success.Value)
        {
            return (int)json.bugs.Value;
        }
        return -1;
    }

    public Plant Save(NewPlantUpdate data)
    {
        string jsonData = JsonUtility.ToJson(data);
        string response = SendRequest(URL + "new_plant/", jsonData, true);
        dynamic json = JObject.Parse(response);
        if (!Authentication(json.authentication.Value))
            return null;
        if (json.success.Value)
        {
            return json["plants"][0].ToObject<Plant>();
        }
        return null;
    }

    public int Save(PurchasetUpdate data, string type)
    {
        string jsonData = JsonUtility.ToJson(data);
        string response = "";
        switch (type)
        {
            case "seed":
                response= SendRequest(URL + "seed_purchase/", jsonData, true);
                break;
        }
        dynamic json = JValue.Parse(response);
        if (!Authentication(json.authentication.Value))
            return -1;
        if (json.success.Value)
        {
            return (int)json.coins.Value;
        }
        return -1;
    }

    public int Save(SellUpdate data)
    {
        string jsonData = JsonUtility.ToJson(data);
        string response = SendRequest(URL + "sell_fetus/", jsonData, true);
        dynamic json = JValue.Parse(response);
        if (!Authentication(json.authentication.Value))
            return -1;
        if (json.success.Value)
        {
            return (int)json.coins.Value;
        }
        return -1;
    }

    public dynamic Save(FetusUpdate data)
    {
        string jsonData = JsonUtility.ToJson(data);
        string response = SendRequest(URL + "collect_fetus/", jsonData, true);
        dynamic json = JValue.Parse(response);
        if (!Authentication(json.authentication.Value))
            return -1;
        if (json.success.Value)
        {
            return json;
        }
        return null;
    }

    public bool Save(ShovelUpdate data)
    {
        string jsonData = JsonUtility.ToJson(data);
        string response = SendRequest(URL + "shovel/", jsonData, true);
        dynamic json = JValue.Parse(response);
        if (!Authentication(json.authentication.Value))
            return false;
        if (json.success.Value)
        {
            return true;
        }
        return false;
    }
}