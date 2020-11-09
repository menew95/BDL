using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI : MonoBehaviour
{

    public Text[] remain;// remina Dirt, Root, Pebble;
    public Text level;
    public Text remainTryTime;

    public Button[] button;
    public GameObject[] block;
    public int currDig = 0; // 0=> swallowly, 1=> deeply
    public Canvas canvas;
    public GameObject[] two;
    Vector3 min, max;
    public Vector3 viewMin, viewMax;
    float yMin, yMax;
    // Start is called before the first frame update
    void Start()
    {
        yMin = two[1].GetComponent<RectTransform>().rect.y * 2;
        yMax = two[0].GetComponent<RectTransform>().rect.y * 2;
        min = Vector3.zero;
        min.y = -yMin;
        max.x = two[1].GetComponent<RectTransform>().rect.xMax * 2;
        max.y = 1920f + yMax;
        viewMin = Camera.main.ScreenToViewportPoint(min);
        viewMax = Camera.main.ScreenToViewportPoint(max);
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.mousePosition);
        //Debug.Log(Camera.main.ViewportToScreenPoint(Vector3.one) + " " +yMax + " " + min + " " + max + " " + viewMin + " " + viewMax + " " + Camera.main.ViewportToWorldPoint(viewMin) + " " + Camera.main.ViewportToWorldPoint(viewMax));
    }

    public void UpdateRemainText(int remainDirt, int remainRoot, int remainPebble)
    {
        remain[0].text = remainDirt.ToString();
        remain[1].text = remainRoot.ToString();
        remain[2].text = remainPebble.ToString();
    }

    public void UpdateRemainTryTime(int _remainTryTime)
    {
        remainTryTime.text = _remainTryTime.ToString();
    }

    public void UpdateLevel(int _level)
    {
        level.text = (_level + 1).ToString() + " / 5";
    }

    public void ChangeDigType()
    {
        button[currDig].enabled = true;
        block[currDig].SetActive(true);

        if (currDig == 0)
        {
            currDig = 1;
        }
        else if (currDig == 1)
        {
            currDig = 0;
        }

        button[currDig].enabled = false;
        block[currDig].SetActive(false);

    }
}
