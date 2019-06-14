using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    GameObjectCollector Collector;
    public string Mode = "";
    public Confirm_button_options options;
    private GameObject grid;
    private GameObject veget;
    private GameObject shovel_button;
    public Seed_info seed_info;
    private Saver_script saver;


    // Start is called before the first frame update
    void Start()
    {
        Collector = options.Collector.GetComponent<GameObjectCollector>();
        saver = Collector.GameObjects.Saver.GetComponent<Saver_script>();
        grid = Collector.GameObjects.Grid;
        veget = Collector.GameObjects.Veget;
        shovel_button = Collector.GameObjects.ShovelButton;
    }

    public void OnMouseDown()
    {
        if (grid.GetComponent<Grid_script>().current == null)
            return;
        switch(Mode)
        {
            case "Plant":
                if (grid.GetComponent<Grid_script>().current.info.plant != 0)
                    break;
                veget.GetComponent<Veget_script>().New_veget(seed_info.plant_type);
                grid.GetComponent<Grid_script>().Clear_grid();
                grid.GetComponent<Grid_script>().Switch("Plant");
                Mode = "";
                veget.SetActive(true);
                this.gameObject.SetActive(false);
                break;
            case "Bed":
                if (grid.GetComponent<Grid_script>().current.info.is_dug_up)
                    break;
                if (Collector.Online)
                    if (saver.Save(new ShovelUpdate(grid.GetComponent<Grid_script>().current.info.id)))
                        grid.GetComponent<Grid_script>().current.info.is_dug_up = true;
                    else
                    grid.GetComponent<Grid_script>().current.info.is_dug_up = true;
                grid.GetComponent<Grid_script>().Clear_grid();
                grid.GetComponent<Grid_script>().Switch("Bed");
                Mode = "";
                veget.SetActive(true);
                shovel_button.GetComponentInParent<Image>().color = new Color(1f, 1f, 1f);
                this.gameObject.SetActive(false);
                break;
        }
    }
}
