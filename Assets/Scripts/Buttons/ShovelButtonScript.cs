using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShovelButtonScript : MonoBehaviour
{
    public GameObjectCollector Collector;
    GameObject confirm_Button;
    Grid_script grid_Script;
    GameObject veget;
    bool isActive = false;


    // Start is called before the first frame update
    void Start()
    {
        confirm_Button = Collector.GameObjects.Confirm;
        grid_Script = Collector.GameObjects.Grid.GetComponent<Grid_script>();
        veget = Collector.GameObjects.Veget;
    }

    public void ButtonClick()
    {
        veget.SetActive(!veget.activeSelf);
        grid_Script.Switch("Bed");
        confirm_Button.gameObject.SetActive(!confirm_Button.activeSelf);
        if (!isActive)
        {
            confirm_Button.GetComponent<Confirm_button>().Mode = "Bed";
            this.GetComponentInParent<Image>().color = new Color(0.387f, 0.922f, 0.996f);
        }
        else
            this.GetComponentInParent<Image>().color = new Color(1f, 1f, 1f);
        isActive = !isActive;
    }
}
