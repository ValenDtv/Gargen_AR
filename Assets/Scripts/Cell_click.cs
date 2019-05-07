using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell_click : MonoBehaviour
{

    public GameObject veget;
    private Grid_script grid;
    public int num;

    // Start is called before the first frame update
    void Start()
    {
        //tree = GameObject.Find("Tree");
        //tree.SetActive(false);
        grid = this.gameObject.GetComponentInParent<Grid_script>();
        veget = grid.veget;
    }


    void OnMouseDown()
    {
        if (!grid.edit_mode)
            return;
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        //this.gameObject.SetActive(false);
        grid.Clear_grid();
        grid.current = grid.game_cells[num];
        //veget.transform.position = this.gameObject.transform.position;
        this.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 1);
        //tree.SetActive(true);
    }
}
