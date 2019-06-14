using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Product_options
{
    public GameObject CheckMark;
    public GameObject Cross;
}

public class Product_selected : MonoBehaviour
{
    public int num;
    public int objnum;
    public Things_panel_script parent;
    public Product_options options;
    private GameObject checkMark;
    private GameObject cross;
    private float checkMarkTime = 0f;
    private float crossTime = 0f;

    private void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(ChangeSelectedNum);
        checkMark = options.CheckMark;
        cross = options.Cross;
    }

    public void ChangeSelectedNum()
    {
        int i = parent.current ?? -1;
        if (i != -1)
            parent.Products[i].GetComponent<Image>().color = new Color(0.441f, 0.816f, 0,078);
        parent.current = num;
        this.gameObject.GetComponent<Image>().color = new Color(0.169f, 0.757f, 0.094f);
        parent.action_button.GetComponent<Button>().interactable = true;
    }

    public void ResultOfAction (bool success)
    {
        if (success)
        {
            checkMark.SetActive(true);
            checkMarkTime = 0.4f;
        }
        else
        {
            cross.SetActive(true);
            crossTime = 0.4f;
        }
    }

    private void Update()
    {
        if (checkMark.activeSelf)
        {
            if (checkMarkTime <= 0f)
            {
                checkMark.SetActive(false);
            }
            else
            {
                checkMarkTime -= Time.deltaTime;
            }
        }
        if (cross.activeSelf)
        {
            if (crossTime <= 0f)
            {
                cross.SetActive(false);
            }
            else
            {
                crossTime -= Time.deltaTime;
            }
        }
    }
}
