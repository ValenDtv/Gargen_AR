using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Thing_panel_options
{
    public GameObject Collector;
}

public class Things_panel_script : MonoBehaviour
{
    public Thing_panel_options options;
    private GameObjectCollector GOC;
    private Saver_script saver;
    private List<Store> store;
    private List<Stock> stock;
    private List<Seeds> seeds;
    private GameObject store_button;
    private GameObject stock_button;
    private GameObject seeds_button;
    private GameObject veget;
    private Confirm_button confirm_button;
    public GameObject action_button;
    private string Mode = "Store";
    public int? current=null;
    public List<GameObject> Products;
    //private  GardenAR_db gardenAR_db = new GardenAR_db();
    private Text coins_text;
    private bool first_appearance = true;

    // Start is called before the first frame update
    void Start()
    {
        GOC = options.Collector.GetComponent<GameObjectCollector>();
        //gardenAR_db = GOC.gardenAR_db;
        saver = GOC.GameObjects.Saver.GetComponent<Saver_script>();
        store = GOC.gardenAR_db.store;
        stock = GOC.gardenAR_db.stock;
        seeds = GOC.gardenAR_db.seeds;
        store_button = GOC.GameObjects.Store_button;
        stock_button = GOC.GameObjects.Stock_button;
        seeds_button = GOC.GameObjects.Seeds_button;
        confirm_button = GOC.GameObjects.Confirm.GetComponent<Confirm_button>();
        action_button = GOC.GameObjects.Action_thing_button;
        coins_text = GOC.GameObjects.Coins.GetComponent<Text>();
        veget = GOC.GameObjects.Veget;
        store_button.GetComponent<Button>().onClick.AddListener(() => Switch(store_button.name));
        stock_button.GetComponent<Button>().onClick.AddListener(() => Switch(stock_button.name));
        seeds_button.GetComponent<Button>().onClick.AddListener(() => Switch(seeds_button.name));
        action_button.GetComponent<Button>().onClick.AddListener(Action);
        action_button.GetComponent<Button>().interactable = false;
        GOC.GameObjects.Exit_thing_button.GetComponent<Button>().onClick.AddListener(Exit_thing_panel);
        Fill_store();
        first_appearance = false;
    }

    public void Fill_store()
    {
        Products = new List<GameObject>();
        for (int i=0;i<store.Count; i++)
        {
            GameObject product = Instantiate(Resources.Load<GameObject>(@"Product"));
            product.transform.SetParent(options.Collector.GetComponent<GameObjectCollector>().GameObjects.Scroll.transform);
            string path = @"Pictures/Store/" + GardenAR_db.GetPlantTypeById(store[i].source).file;
            product.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
            product.transform.GetComponentInChildren<Text>().text = GardenAR_db.GetPlantTypeById(store[i].source).name + "\n Цена:"
                + GardenAR_db.GetPlantTypeById(store[i].source).seed_priсe.ToString();
            product.transform.localScale = new Vector3(1, 1, 1);
            //product.AddComponent<Product_selected>();
            product.GetComponent<Product_selected>().parent = this;
            product.GetComponent<Product_selected>().num = i;
            Products.Add(product);
        }
    }

    public void Fill_stock()
    {
        Products = new List<GameObject>();
        for (int i = 0; i < stock.Count; i++)
        {
            GameObject product = Instantiate(Resources.Load<GameObject>(@"Product"));
            product.transform.SetParent(options.Collector.GetComponent<GameObjectCollector>().GameObjects.Scroll.transform);
            string path = @"Pictures/Stock/" + GardenAR_db.GetPlantTypeById(stock[i].plant_type).file;
            product.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
            product.transform.GetComponentInChildren<Text>().text = GardenAR_db.GetPlantTypeById(stock[i].plant_type).fetus
                + "\n Колличество:" + stock[i].count.ToString();
            //product.AddComponent<Product_selected>();
            product.transform.localScale = new Vector3(1, 1, 1);
            product.GetComponent<Product_selected>().parent = this;
            product.GetComponent<Product_selected>().num = i;
            Products.Add(product);
        }
    }

    public void Fill_seeds()
    {
        Products = new List<GameObject>();
        for (int i = 0; i < seeds.Count; i++)
        {
            GameObject product = Instantiate(Resources.Load<GameObject>(@"Product"));
            product.transform.SetParent(options.Collector.GetComponent<GameObjectCollector>().GameObjects.Scroll.transform);
            string path = @"Pictures/Seeds/" + GardenAR_db.GetPlantTypeById(seeds[i].plant_type).file;
            product.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
            product.transform.GetComponentInChildren<Text>().text = GardenAR_db.GetPlantTypeById(seeds[i].plant_type).name + "\n Колличество:"
                + seeds[i].count.ToString();
            product.transform.localScale = new Vector3(1, 1, 1);
            //product.AddComponent<Product_selected>();
            product.GetComponent<Product_selected>().parent = this;
            product.GetComponent<Product_selected>().num = i;
            Products.Add(product);
        }
    }

    public void Switch(string mode)
    {
        if (this.Mode == mode)
            return;
        this.Mode = mode;
        foreach (GameObject product in Products)
            DestroyImmediate(product);
        this.current = null;
        action_button.GetComponent<Button>().interactable = false;
        store_button.GetComponent<Image>().color = new Color(1, 1, 1);
        stock_button.GetComponent<Image>().color = new Color(1, 1, 1);
        seeds_button.GetComponent<Image>().color = new Color(1, 1, 1);
        switch (mode)
        {
            case "Store":
                Fill_store();
                action_button.GetComponentInChildren<Text>().text = "Купить";
                store_button.GetComponent<Image>().color = new Color(0.387f, 0.922f, 0.996f);
                break;
            case "Stock":
                Fill_stock();
                action_button.GetComponentInChildren<Text>().text = "Продать";
                stock_button.GetComponent<Image>().color = new Color(0.387f, 0.922f, 0.996f);
                break;
            case "Seeds":
                Fill_seeds();
                action_button.GetComponentInChildren<Text>().text = "Посадить";
                seeds_button.GetComponent<Image>().color = new Color(0.387f, 0.922f, 0.996f);
                break;
        }
    }

    public void Action()
    {
        if (this.current == null)
            return;
        switch(this.Mode)
        {
            case "Store":
                Buy();
                break;
            case "Stock":
                Sell();
                break;
            case "Seeds":
                Plant();
                break;
        }
    }

    public void Exit_thing_panel()
    {
        this.gameObject.SetActive(false);
    }

    private void Buy()
    {
        int num = current ?? -1;
        if (num == -1)
            return;
        if (GardenAR_db.GetPlantTypeById(store[num].source).seed_priсe > GOC.gardenAR_db.coins)
        {
            Products[num].GetComponent<Product_selected>().ResultOfAction(false);
            return;
        }
        switch(store[num].type)
        {
            case "seed":
                Products[num].GetComponent<Product_selected>().ResultOfAction(true);
                bool flag = false;
                foreach (Seeds sd in seeds)
                    if (sd.plant_type == store[num].source)
                    {
                        sd.count++;
                        flag = true;
                    }
                if (!flag)
                    seeds.Add(new Seeds(store[num].source));
                break;
        }
        GOC.gardenAR_db.coins -= GardenAR_db.GetPlantTypeById(store[num].source).seed_priсe;
        GOC.GameObjects.Coins.GetComponent<Text>().text = GOC.gardenAR_db.coins.ToString();
        saver.Save_DB();
    }

    private void Sell()
    {
        int num = current ?? -1;
        if (num == -1)
            return;
        stock[num].count--;
        GOC.gardenAR_db.coins++;
        Products[num].GetComponent<Product_selected>().ResultOfAction(true);
        if (stock[num].count < 1)
        {
            stock.Remove(stock[num]);
            Destroy(Products[num]);
            this.current = null;
        }
        else
        {
            Products[num].transform.GetComponentInChildren<Text>().text = GardenAR_db.GetPlantTypeById(stock[num].plant_type).name
                + "\n Колличество:" + stock[num].count.ToString();
        }
        GOC.GameObjects.Coins.GetComponent<Text>().text = GOC.gardenAR_db.coins.ToString();
        saver.Save_DB();
    }

    private void Plant()
    {
        int num = current ?? -1;
        if (num == -1)
            return;
        veget.SetActive(false);
        GOC.GameObjects.Grid.GetComponent<Grid_script>().Switch();
        confirm_button.Mode = "Plant";
        confirm_button.seed_info = new Confirm_button.Seed_info(seeds[num].plant_type);
        confirm_button.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (first_appearance)
            return;
        foreach (GameObject product in Products)
            DestroyImmediate(product);
        switch (Mode)
        {
            case "Store":
                Fill_store();
                break;
            case "Stock":
                Fill_stock();
                break;
            case "Seeds":
                Fill_seeds();
                break;
        }
    }
}
