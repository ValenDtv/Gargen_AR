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
    public bool authentication;
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

    public ThirstUpdate(int userid, int plantid)
    {
        this.userid = userid;
        this.plantid = plantid;
    }
}


[System.Serializable]
public class BugsUpdate
{
    public int userid;
    public int plantid;

    public BugsUpdate(int userid, int plantid)
    {
        this.userid = userid;
        this.plantid = plantid;
    }
}


[System.Serializable]
public class NewPlantUpdate
{
    public int userid;
    public int type_id;
    public int cellid;

    public NewPlantUpdate(int userid, int type_id, int cellid)
    {
        this.userid = userid;
        this.type_id = type_id;
        this.cellid = cellid;
    }
}


[System.Serializable]
public class PurchasetUpdate
{
    public int userid;
    public int source;

    public PurchasetUpdate(int userid, int source)
    {
        this.userid = userid;
        this.source = source;
    }
}


[System.Serializable]
public class SellUpdate
{
    public int userid;
    public int plant_type;

    public SellUpdate(int userid, int plant_type)
    {
        this.userid = userid;
        this.plant_type = plant_type;
    }
}

[System.Serializable]
public class FetusUpdate
{
    public int userid;
    public int plantid;

    public FetusUpdate(int userid, int plantid)
    {
        this.userid = userid;
        this.plantid = plantid;
    }
}

[System.Serializable]
public class RegistrationData
{
    public string login;
    public string password;
    public string email;

    public RegistrationData(string login, string password, string email)
    {
        this.login = login;
        this.password = password;
        this.email = email;
    }
}


[System.Serializable]
public class ShovelUpdate
{
    public int cellid;

    public ShovelUpdate(int cellid)
    {
        this.cellid = cellid;
        this.cellid = cellid;
    }
}
