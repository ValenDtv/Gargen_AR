using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Veget_options
{
    public GameObject Collector;

}

public class Veget_script : MonoBehaviour
{

    public class Veget_unit
    {
        internal GameObject veg_unit;
        internal Plant info;
        internal GameObject waterDrops;
        internal GameObject bugs;

        public Veget_unit(GameObject veg_unit, Plant info)
        {
            this.veg_unit = veg_unit;
            this.info = info;
        }
    }

    public Veget_options options;
    private GameObjectCollector Collector;
    private Grid_script grid;
    private GardenAR_db gardenAR_db;
    private ParticleSystem wateringCan;
    List<Veget_unit> vegets = new List<Veget_unit>();
    private Things_panel_script thingPanel;
    private Saver_script saver;


    // Start is called before the first frame update
    void Start()
    {
        Collector = options.Collector.GetComponent<GameObjectCollector>();
        grid = Collector.GameObjects.Grid.GetComponent<Grid_script>();
        gardenAR_db = Collector.gardenAR_db;
        wateringCan = Collector.GameObjects.WateringCan.GetComponent<ParticleSystem>();
        thingPanel = Collector.GameObjects.Thing_panel.GetComponent<Things_panel_script>();
        saver = Collector.GameObjects.Saver.GetComponent<Saver_script>();

        for (int i = 0; i < gardenAR_db.plants.Count; i++)
        {
            string path = GardenAR_db.GetPlantTypeById(gardenAR_db.plants[i].type_id).file+@"\" + 
                GardenAR_db.GetPlantTypeById(gardenAR_db.plants[i].type_id).file + gardenAR_db.plants[i].stage.ToString();
            GameObject temp_veg = Instantiate(Resources.Load<GameObject>(@"Models\"+path));
            temp_veg.transform.SetParent(this.gameObject.transform);
            temp_veg.name = i.ToString();
            temp_veg.transform.localEulerAngles = new Vector3(0, 0, 0);
            temp_veg.transform.localPosition = new Vector3(0, 0, 0);
            temp_veg.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            temp_veg.transform.position = grid.game_cells[gardenAR_db.plants[i].cell - 1].plane.transform.position;
            temp_veg.AddComponent<VegetUnit_script>().Collector = options.Collector.GetComponent<GameObjectCollector>();
            temp_veg.layer = 9;
            temp_veg.SetActive(true);
            vegets.Add(new Veget_unit(temp_veg, gardenAR_db.plants[i]));

            WaterDropsInit(i);
            BugsInit(i);
        }
    }

    public void New_veget(int plant_type)
    {
        Grid_script.Game_cell current_cell = grid.current;
        Plant new_veg_info = saver.Save(new NewPlantUpdate(Collector.Userid, plant_type, current_cell.info.id));
        if (new_veg_info == null)
            return;
        GameObject new_veg;
        current_cell.info.plant = new_veg_info.id;
        //new_veg =  Instantiate(Resources.Load<GameObject>(@"Models\Tree\Tree1"));
        PlantType veg_type = GardenAR_db.GetPlantTypeById(new_veg_info.type_id);
        new_veg = Instantiate(Resources.Load<GameObject>(@"Models\" + veg_type.file + @"\" + veg_type.file + @"1"));
        new_veg.name = gardenAR_db.plants.Count.ToString();
        new_veg.transform.SetParent(this.gameObject.transform);
        new_veg.transform.localEulerAngles = new Vector3(0, 0, 0);
        new_veg.transform.localPosition = new Vector3(0, 0, 0);
        new_veg.transform.position = current_cell.plane.transform.position;
        new_veg.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        new_veg_info.cell = grid.current.info.id;
        new_veg.AddComponent<VegetUnit_script>().Collector = options.Collector.GetComponent<GameObjectCollector>();
        new_veg.layer = 9;
        new_veg.SetActive(true);
        vegets.Add(new Veget_unit(new_veg, new_veg_info));
        gardenAR_db.plants.Add(new_veg_info);
        int num = thingPanel.current ?? -1;
        if (num != -1)
        {
            int seednum = thingPanel.Products[num].GetComponent<Product_selected>().objnum;
            gardenAR_db.seeds[seednum].count--;
            if (gardenAR_db.seeds[seednum].count < 1)
            {
                thingPanel.current = null;
            }
        }
        saver.Save_DB();
    }

    public void WaterDropsInit(int num)
    {
        if (vegets[num].waterDrops != null)
            GameObject.Destroy(vegets[num].waterDrops);

        GameObject sprits_continer = new GameObject();
        vegets[num].waterDrops = sprits_continer;
        sprits_continer.transform.SetParent(vegets[num].veg_unit.transform);
        sprits_continer.transform.localEulerAngles = new Vector3(0, 0, 0);
        sprits_continer.transform.localPosition = new Vector3(0, 7, 0);
        sprits_continer.transform.localScale = new Vector3(1f, 1f, 1f);

        float start = vegets[num].info.thirst%2 == 0 ? 0.4f : 0f;
        for (int j = 0; j < vegets[num].info.thirst; j++)
        {
            GameObject sprite = Instantiate(Resources.Load<GameObject>("WaterDropSprite"));
            sprite.transform.SetParent(sprits_continer.transform);
            sprite.transform.localEulerAngles = new Vector3(0, 0, 0);
            float position_x = start + 0.6f * Mathf.CeilToInt(j / 2f) * Mathf.Pow(-1, j);
            sprite.transform.localPosition = new Vector3(position_x, 0, 0);
            sprite.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            sprite.GetComponent<CameraFacingBillboard>().m_Camera = options.Collector.GetComponent<GameObjectCollector>()
                .GameObjects.AR_camera.GetComponent<Camera>();
            sprite.SetActive(true);
        }
    }

    public void BugsInit(int num)
    {
        if (vegets[num].bugs != null)
            Destroy(vegets[num].bugs);

        GameObject sprits_continer = new GameObject();
        vegets[num].bugs = sprits_continer;
        sprits_continer.transform.SetParent(vegets[num].veg_unit.transform);
        sprits_continer.transform.localEulerAngles = new Vector3(0, 0, 0);
        sprits_continer.transform.localPosition = new Vector3(0, 8, 0);
        sprits_continer.transform.localScale = new Vector3(1f, 1f, 1f);

        float start = vegets[num].info.bugs % 2 == 0 ? 0.4f : 0f;
        for (int j = 0; j < vegets[num].info.bugs; j++)
        {
            GameObject sprite = Instantiate(Resources.Load<GameObject>("BugSprite"));
            sprite.transform.SetParent(sprits_continer.transform);
            sprite.transform.localEulerAngles = new Vector3(0, 0, 0);
            float position_x = start + 0.7f * Mathf.CeilToInt(j / 2f) * Mathf.Pow(-1, j);
            sprite.transform.localPosition = new Vector3(position_x, 0, 0);
            sprite.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            sprite.GetComponent<CameraFacingBillboard>().m_Camera = options.Collector.GetComponent<GameObjectCollector>()
                .GameObjects.AR_camera.GetComponent<Camera>();
            sprite.SetActive(true);
        }
    }
}
