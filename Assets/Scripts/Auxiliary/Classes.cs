using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GardenAR_db
{
    public List<Plant> plants;
    public Cell[] cells;
    public List<Store> store;
    public List<Stock> stock;
    public List<Seeds> seeds;
    public int coins;
}


[System.Serializable]
public class Plant
{
    public int id;
    public string file;
    public int cell;
    public int stage;
    public string fetus;
    public int points;
    public int max_points;
    public int thirst;
    public int bugs;
    public string create;
    public string update;


    public Plant(int id, string file, string fetus, int max_points)
    {
        this.id = id;
        this.file = file;
        this.stage = 1;
        this.points = 0;
        this.max_points = max_points;
        this.bugs = 0;
        this.thirst = 0;
        this.create = System.DateTime.Now.ToString();
        this.update = System.DateTime.Now.ToString();
        this.fetus = fetus;
    }
}

[System.Serializable]
public class Cell
{
    public int id;
    public bool is_dug_up;
    public int plant;
    public bool is_fertilized;
}

[System.Serializable]
public class Store
{
    public string name;
    public int price;
    public string type;
    public string file;
    public string fetus;
    public int max_points;
}

[System.Serializable]
public class Stock
{
    public string name;
    public int count;
    public string file;

    public Stock(string name, string file, int count)
    {
        this.name = name;
        this.file = file;
        this.count = count;
    }
}

[System.Serializable]
public class Seeds
{
    public string name;
    public string file;
    public string fetus;
    public int max_points;
    public int count;

    public Seeds(string name, string file, string fetus, int max_points)
    {
        this.name = name;
        this.file = file;
        this.count = 1;
        this.fetus = fetus;
        this.max_points = max_points;
    }
}