using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Confirm_button_options
{
    public GameObject Collector;
}

public class Confirm_button : MonoBehaviour
{
    public struct Seed_info
    {
        public int plant_type;

        public Seed_info(int plant_type)
        {
            this.plant_type = plant_type;
        }
    }

    public string Mode = "";
    public Confirm_button_options options;
    private GameObject grid;
    private GameObject veget;

    public Seed_info seed_info;


    // Start is called before the first frame update
    void Start()
    {
        grid = options.Collector.GetComponent<GameObjectCollector>().GameObjects.Grid;
        veget = options.Collector.GetComponent<GameObjectCollector>().GameObjects.Veget;
    }

    public void OnMouseDown()
    {
        switch(Mode)
        {
            case "Plant":
                if (grid.GetComponent<Grid_script>().current.info.plant != 0)
                    break;
                veget.GetComponent<Veget_script>().New_veget(seed_info.plant_type);
                grid.GetComponent<Grid_script>().Clear_grid();
                grid.GetComponent<Grid_script>().Switch();
                Mode = "";
                veget.SetActive(true);
                this.gameObject.SetActive(false);
                break;
            case "Bed":
                //Вызов функции вскопки грядки
                break;
        }
    }
}
