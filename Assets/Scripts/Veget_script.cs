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
    private Grid_script grid;
    private GardenAR_db gardenAR_db;
    private ParticleSystem wateringCan;
    List<Veget_unit> vegets = new List<Veget_unit>();
    //private List<GameObject> waterDrops;
    //private List<GameObject> bugs;
    private Things_panel_script thingPanel;
    private Saver_script saver;


    // Start is called before the first frame update
    void Start()
    {
        grid = options.Collector.GetComponent<GameObjectCollector>().GameObjects.Grid.GetComponent<Grid_script>();
        gardenAR_db = options.Collector.GetComponent<GameObjectCollector>().gardenAR_db;
        wateringCan = options.Collector.GetComponent<GameObjectCollector>().GameObjects.WateringCan.GetComponent<ParticleSystem>();
        thingPanel = options.Collector.GetComponent<GameObjectCollector>().GameObjects.Thing_panel.GetComponent<Things_panel_script>();
        saver = options.Collector.GetComponent<GameObjectCollector>().GameObjects.Saver.GetComponent<Saver_script>();

        for (int i = 0; i < gardenAR_db.plants.Count; i++)
        {
            string path = gardenAR_db.plants[i].file+@"\"+gardenAR_db.plants[i].file+gardenAR_db.plants[i].stage.ToString();
            GameObject temp_veg = Instantiate(Resources.Load<GameObject>(@"Models\"+path));
            temp_veg.transform.SetParent(this.gameObject.transform);
            temp_veg.name = i.ToString();
            temp_veg.transform.localEulerAngles = new Vector3(0, 0, 0);
            temp_veg.transform.localPosition = new Vector3(0, 0, 0);
            temp_veg.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            temp_veg.transform.position = grid.game_cells[gardenAR_db.plants[i].cell - 1].plane.transform.position;
            temp_veg.AddComponent<VegetUnit_script>().Collector = options.Collector.GetComponent<GameObjectCollector>();
            //wateringCan.collision.SetPlane(i+1, temp_veg.transform);
            temp_veg.layer = 9;
            temp_veg.SetActive(true);
            vegets.Add(new Veget_unit(temp_veg, gardenAR_db.plants[i]));

            WaterDropsInit(i);
            BugsInit(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Функция для добавления расстения
    public void New_veget(string file, string fetus, int max_points)
    {
        GameObject new_veg;
        Plant new_veg_info = new Plant(gardenAR_db.plants.Count + 1, file, fetus, max_points);
        Grid_script.Game_cell current_cell = grid.current;
        current_cell.info.is_dug_up = true;
        current_cell.info.plant = gardenAR_db.plants.Count + 1;
        //this.gameObject.transform.Find("Tree").gameObject.SetActive(true);
        new_veg =  Instantiate(Resources.Load<GameObject>(@"Models\Tree\Tree1"));
        new_veg.name = gardenAR_db.plants.Count.ToString();
        new_veg.transform.SetParent(this.gameObject.transform);
        //Tree.transform.position = new Vector3(0, 0, 0);
        //Tree.transform.localEulerAngles.Set(0, 0, 0);
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
        //Уменьшаем число доступных для посадки семян
        int num = thingPanel.current ?? -1;
        gardenAR_db.seeds[num].count--;
        if (gardenAR_db.seeds[num].count < 1)
        {
            gardenAR_db.seeds.Remove(gardenAR_db.seeds[num]);
            //Destroy(Products[num]);
            thingPanel.current = null;
        }
        //wateringCan.collision.SetPlane(vegets.Count, new_veg.transform);
        saver.Save_DB();
    }

    public void WaterDropsInit(int num)
    {
        //waterDrops = new List<GameObject>();
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
            //waterDrops.Add(sprite);
        }
    }

    public void BugsInit(int num)
    {
        //bugs = new List<GameObject>();
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
            //bugs.Add(sprite);
        }
    }
}
