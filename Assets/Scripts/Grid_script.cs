using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Options
{
    public int Dimension;
    public float Plane_size;
    public GameObject Collector;
    
}


public class Grid_script : MonoBehaviour
{
    public class Game_cell
    {
       internal GameObject plane;
       internal Cell info;
       internal MeshRenderer meshRenderer;

        public Game_cell (GameObject plane, Cell info, MeshRenderer meshRenderer)
        {
            this.plane = plane;
            this.info = info;
            this.meshRenderer = meshRenderer;
        }
    }

    public Game_cell[] game_cells;
    public Options options;
    private int dimension;
    private float plane_size;
    public GameObject veget;
    private GardenAR_db gardenAR_db;
    public bool edit_mode = false;
    public Game_cell current;
    Material GardenBed;
    Material GardenBedCircuit;
    Material Circuit;
    //private UnityEvent grid_switch;


    // Start is called before the first frame update
    void Start()
    {
        dimension = options.Dimension;
        plane_size = options.Plane_size;
        game_cells = new Game_cell[dimension*dimension];
        veget = options.Collector.GetComponent<GameObjectCollector>().GameObjects.Veget;
        gardenAR_db = options.Collector.GetComponent<GameObjectCollector>().gardenAR_db;
        GardenBed = Resources.Load<Material>(@"Materials\Garden_bed");
        GardenBedCircuit = Resources.Load<Material>(@"Materials\Garden_bed_circuit");
        Circuit = Resources.Load<Material>(@"Materials\circuit");
        //if (grid_switch == null)
        //    grid_switch = new UnityEvent();
        //grid_switch.AddListener(Switch);
        //GameObject.Find("Grid_button").GetComponent<Button>().onClick.AddListener(delegate { Switch(); });
        //События для кнопки, которая отображает сетку
        //GameObject.Find("Grid_button").GetComponent<Button>().onClick.AddListener(Switch);

        //GameObject plane_test = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //plane_test.transform.position = new Vector3(0, 1, 0);
        //plane_test.transform.localScale = new Vector3(plane_size, plane_size, plane_size);

        for (int i=0; i < dimension; i++)
            for (int j=0; j< dimension; j++)
            {
                //Создаём клетку
                GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                plane.transform.position = new Vector3((-dimension/2 + j*(plane_size/0.2f))*2, 0.001f, (-dimension/2 + i * (plane_size/0.2f)) *2);
                //plane.transform.localPosition = new Vector3((-dimension / 2 + j) * 2, 0.001f, (-dimension / 2 + i) * 2);
                plane.transform.localScale = new Vector3(plane_size,plane_size,plane_size);
                //Добавляем текстуру обводки к клетке
                MeshRenderer meshRenderer = plane.GetComponent<MeshRenderer>();
                //meshRenderer.material = GardenBed;
                //Поставим коллайдер в режим триггера или выключаем его вообще
                //plane.GetComponent<Collider>().enabled = false;
                plane.GetComponent<MeshCollider>().convex = true;
                plane.GetComponent<MeshCollider>().isTrigger = true;
                //Добавляем скрипт обработки нажатия
                plane.AddComponent<Cell_click>();
                plane.GetComponent<Cell_click>().num = j + (i * dimension);
                //plane.GetComponent
                //Присоединяем к решётки
                plane.transform.SetParent(this.gameObject.transform);
                Game_cell game_Cell = new Game_cell(plane, gardenAR_db.cells[j + (i*dimension)], meshRenderer);
                game_cells[j + (i * dimension)] = game_Cell;
                if (game_cells[j + (i * dimension)].info.is_dug_up)
                    meshRenderer.material = GardenBed;
                else
                {
                    game_cells[j + (i * dimension)].plane.SetActive(false);
                    meshRenderer.material = Circuit;
                }
                //if (!game_cells[j + (i * dimension)].info.is_dug_up)
                //    game_cells[j + (i * dimension)].plane.SetActive(false);
            }
    }

    public void Clear_grid ()
    {
        for (int i = 0; i < game_cells.Length; i++)
            if (!game_cells[i].info.is_dug_up)
                game_cells[i].meshRenderer.material = Circuit;
            else
            {
                if (edit_mode)
                {
                    game_cells[i].meshRenderer.material = GardenBedCircuit;
                    if (game_cells[i].info.plant != 0)
                        game_cells[i].meshRenderer.material.color = new Color(0f, 0.5f, 0);
                }
                else
                {
                    game_cells[i].meshRenderer.material = GardenBed;
                }
            }
    }

    public void Switch()
    {
        edit_mode = !edit_mode;
        for (int i = 0; i < game_cells.Length; i++)
        {
            if (!game_cells[i].info.is_dug_up)
                game_cells[i].plane.SetActive(!game_cells[i].plane.activeSelf);
            if (edit_mode)
                game_cells[i].meshRenderer.material = GardenBedCircuit;
            else
                game_cells[i].meshRenderer.material = GardenBed;
        }
        Clear_grid();    
        current = null;

        //this.gameObject.SetActive(!this.gameObject.activeSelf);

                    //tree.SetActive(!tree.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
