using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GardenAR_db
{
    public List<PlantType> plants_type;
    public List<Plant> plants;
    public Cell[] cells;
    public List<Store> store;
    public List<Stock> stock;
    public List<Seeds> seeds;
    public int coins;
    public static GardenAR_db instance;

    public static PlantType GetPlantTypeById (int id)
    {
        foreach (PlantType plantType in instance.plants_type)
            if (plantType.id == id)
                return plantType;
        return null;
    }
}


[System.Serializable]
public class PlantType
{
    public int id;
    public string file;
    public string fetus;
    public int max_points;
    public string name;
    public int seed_priсe;
    public int fetus_price;
    public int fetus_min;
    public int fetus_max;
}


[System.Serializable]
public class Plant
{
    public int id;
    public int type_id;
    public int cell;
    public int stage;
    public int points;
    public int thirst;
    public int bugs;
    public string create;
    public string update;

    public Plant(int id, int type_id)
    {
        this.id = id;
        this.type_id = type_id;
        this.stage = 1;
        this.points = 0;
        this.bugs = 0;
        this.thirst = 0;
        this.create = System.DateTime.Now.ToString();
        this.update = System.DateTime.Now.ToString();
    }
}

[System.Serializable]
public class Cell
{
    public int id;
    public bool is_dug_up;
    public int plant;
    public bool is_fertilized;
    public bool weed;
    public int grass_stage;
    public string dug_up_time;
    public string fertil_time;
}

[System.Serializable]
public class Store
{
    public string type;
    public int source;
}

[System.Serializable]
public class Stock
{
    public int plant_type;
    public int count;

    public Stock(int plant_type, int count)
    {
        this.plant_type = plant_type;
        this.count = count;
    }
}

[System.Serializable]
public class Seeds
{
    public int plant_type;
    public int count;

    public Seeds(int plant_type)
    {
        this.plant_type = plant_type;
        this.count = 1;
    }
}


[System.Serializable]
public class ThirstUpdate
{
    public int userid;
    public int plantid;
    public int thirst;

    public ThirstUpdate(int userid, int plantid, int thirst)
    {
        this.userid = userid;
        this.plantid = plantid;
        this.thirst = thirst;
    }
}
