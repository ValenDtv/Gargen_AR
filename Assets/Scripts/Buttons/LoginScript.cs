using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using UnityEngine.SceneManagement;

public class LoginScript : MonoBehaviour
{
    public GameObject Collector;
    private GameObjectCollector GOC;
    private TMP_InputField login;
    private TMP_InputField password;
    private Text error;
    private TMP_InputField reglogin;
    private TMP_InputField regpassword;
    private TMP_InputField regemail;
    private Text regerror;
    private string path;
    private bool firstAppearance = true;
    public bool test = true;
    //private string URL = "http://127.0.0.1:8000/";
    private string URL = "http://194.113.104.140:8000/";

    // Start is called before the first frame update
    void Start()
    {
        path = Application.persistentDataPath + @"/token.txt";
        if (File.Exists(path))
        {
            string token = "";
            using (StreamReader sr = new StreamReader(path))
            {
                token += sr.ReadToEnd();
            }
            if (automatic_login(token))
            {
                SceneManager.LoadScene("MainScene");
                return;
            }
        }
        GOC = Collector.GetComponent<GameObjectCollector>();
        login = GOC.GameObjects.Login.GetComponent<TMP_InputField>();
        password = GOC.GameObjects.Password.GetComponent<TMP_InputField>();
        error = GOC.GameObjects.ErrorText.GetComponent<Text>();
        GOC.GameObjects.LoginWindow.SetActive(true);
        error.text = "";
        login.text = "";
        password.text = "";
    }

    private string SendRequest(string url, string message, bool authentication)
    {
        using (var webClient = new WebClient())
        {
            string response = webClient.UploadString(url, message);
            return response;
        }

    }

    private bool automatic_login(string token)
    {
        string response = "";
        using (var webClient = new WebClient())
        {
            webClient.Headers.Add(HttpRequestHeader.Authorization, token);
            response = webClient.DownloadString(URL + "automatic_login/");
        }
        dynamic json = JValue.Parse(response);
        return (bool)json.authentication.Value;
    }

    public void EnterButtonClick()
    {
        if (login.text.Length < 5 && !test)
        {
            error.text = "Логин должен иметь не менее 5 симоволов";
            return;
        }
        if (password.text.Length < 8 && !test)
        {
            error.text = "Пароль должен иметь не менее 8 симоволов";
            return;
        }
        string l = "login=" + login.text;
        string p = "password=" + password.text;
        string response;
        using (var webClient = new WebClient())
        {
            response = webClient.DownloadString(URL + "authorization/" + "?" + l + "&" + p);
        }
        dynamic json = JValue.Parse(response);
        if (json.success.Value)
        {
            using (StreamWriter sr = new StreamWriter(path))
            {
                sr.Write((string)json.token.Value);
            }
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            error.text = (string)json.error.Value;
        }
    }

    public void BackToLoginWindow()
    {
        GOC.GameObjects.RegistrationWindow.SetActive(false);
        GOC.GameObjects.LoginWindow.SetActive(true);     
    }

    public void RegistrationButtonClick()
    {
        if (firstAppearance)
        {
            reglogin = GOC.GameObjects.RegistrationWindow.transform.Find("Login").GetComponent<TMP_InputField>();
            regpassword = GOC.GameObjects.RegistrationWindow.transform.Find("Password").GetComponent<TMP_InputField>();
            regemail = GOC.GameObjects.RegistrationWindow.transform.Find("Email").GetComponent<TMP_InputField>();
            regerror = GOC.GameObjects.RegistrationWindow.transform.Find("ErrorText").GetComponent<Text>();
            firstAppearance = false;
        }
        regerror.text = "";
        reglogin.text = "";
        regpassword.text = "";
        regemail.text = "";
        GOC.GameObjects.LoginWindow.SetActive(false);
        GOC.GameObjects.RegistrationWindow.SetActive(true);
    }

    public void ConfirmButtonClick()
    {
        if (reglogin.text.Length < 5 && !test)
        {
            regerror.text = "Логин должен иметь не менее 5 симоволов";
            return;
        }
        if (regpassword.text.Length < 8 && !test)
        {
            regerror.text = "Пароль должен иметь не менее 8 симоволов";
            return;
        }
        string jsonData = JsonUtility.ToJson(new RegistrationData(reglogin.text, regpassword.text, regemail.text));
        string response = SendRequest(URL + "registration/", jsonData, true);
        dynamic json = JValue.Parse(response);
        if (json.success.Value)
        {
            BackToLoginWindow();
            GOC.GameObjects.RegistrationSuccess.SetActive(true);
        }
        else
        {
            regerror.text = (string)json.error.Value;
        }
    }


}
