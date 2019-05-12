using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Net;

[System.Serializable]
public class GameObjects
{
    public GameObject Veget;
    public GameObject Grid;
    public GameObject Confirm;
    public GameObject Thing_panel;
    public GameObject Scroll;
    public GameObject Store_button;
    public GameObject Stock_button;
    public GameObject Seeds_button;
    public GameObject Action_thing_button;
    public GameObject Exit_thing_button;
    public GameObject AR_camera;
    public GameObject ImageTarget;
    public GameObject WateringCan;
    public GameObject WateringCan_button;
    public GameObject Saver;
    public GameObject Coins;
    public GameObject Insecticides_button;
}


    public class GameObjectCollector : MonoBehaviour
{
    public bool Save = true;
    public bool Online = false;
    public GameObjects GameObjects;
    public GardenAR_db gardenAR_db;
    private string path;
    private int userid = 1;
    private string url = "http://127.0.0.1:8000/garden/";
    public int Userid
    {
        get
        {
            return userid;
        }
    }

    void Start()
    {
        //#if UNITY_EDITOR
        //        path = Application.streamingAssetsPath + @"/GardenAR_db.json";
        //#endif
        //#if UNITY_ANDROID
        //        path = "jar:file://" + Application.dataPath + "!/assets/GardenAR_db.json";
        //#endif
        //        Load_DB();
        path = Application.persistentDataPath + @"/GardenAR_db.json";
        Load_DB();
        GameObjects.Coins.GetComponent<Text>().text = gardenAR_db.coins.ToString();
    }

    private void Update()
    {
        //gardenAR_db.plants[0].bugs = 4;
        //GameObjects.Saver.GetComponent<Saver_script>().Save_DB();
    }

    public void Load_DB()
    {
        //        string json = "";
        //#if UNITY_EDITOR
        //        using (StreamReader sr = new StreamReader(path))
        //        {
        //            json += sr.ReadToEnd();
        //        }
        //        //json = Resources.Load<TextAsset>("GardenAR_db").text;
        //#endif
        //#if UNITY_ANDROID
        //        WWW reader = new WWW(path);
        //        while (reader.isDone) { };
        //        json = reader.text;
        //#endif
        //        gardenAR_db = JsonUtility.FromJson<GardenAR_db>(json);

        string json = "";
        if (File.Exists(path) && Save)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                json += sr.ReadToEnd();
            }
        }
        else if (Online)
        {
            string data = "userid="+userid.ToString();
            using (var webClient = new WebClient())
            {
                // Выполняем запрос по адресу и получаем ответ в виде строки
                json = webClient.DownloadString(url+"?"+data);
                Debug.Log(json);
            }
        }
        else
        {
            json = Resources.Load<TextAsset>("GardenAR_db").text;
        }
        gardenAR_db = JsonUtility.FromJson<GardenAR_db>(json);
        GardenAR_db.instance = gardenAR_db;
    }
}
