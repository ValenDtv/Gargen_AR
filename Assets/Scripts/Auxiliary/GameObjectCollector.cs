using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Net;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;

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
    public GameObject LoginWindow;
    public GameObject Login;
    public GameObject Password;
    public GameObject ErrorText;
    public GameObject RegistrationWindow;
    public GameObject ShovelButton;
    public GameObject Canvas;
    public GameObject MenuCanvas;
    public GameObject RegistrationSuccess;
}


    public class GameObjectCollector : MonoBehaviour
{
    public bool Save = true;
    public bool Online = false;
    public bool MainScene = true;
    public GameObjects GameObjects;
    public GardenAR_db gardenAR_db;
    private string path;
    private string token = "";
    private string tokenpath;
    private int userid = 1;
    //private string url = "http://127.0.0.1:8000/garden/";
    private string url = "http://194.113.104.140:8000/garden/";


    public int Userid
    {
        get
        {
            return userid;
        }
    }
    public string Token
    {
        get
        {
            return token;
        }
    }

    void Start()
    {
        if (MainScene)
        {
            path = Application.persistentDataPath + @"/GardenAR_db.json";
            tokenpath = Application.persistentDataPath + @"/token.txt";
            Load_DB();
            GameObjects.Coins.GetComponent<Text>().text = gardenAR_db.coins.ToString();
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

    public void Load_DB()
    {
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
            if (File.Exists(tokenpath))
            {
                using (StreamReader sr = new StreamReader(tokenpath))
                {
                    token += sr.ReadToEnd();
                }
            }
            else
            {
                SceneManager.LoadScene("LoginScene");
                return;
            }
            string data = "userid=" + userid.ToString();
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add(HttpRequestHeader.Authorization, token);
                json = webClient.DownloadString(url+"?"+data);
            }
            dynamic check = JValue.Parse(json);
            if (!Authentication(check.authentication.Value))
                return;
        }
        else
        {
            json = Resources.Load<TextAsset>("GardenAR_db").text;
        }
        gardenAR_db = JsonUtility.FromJson<GardenAR_db>(json);
        GardenAR_db.instance = gardenAR_db;
    }
}
