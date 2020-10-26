using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI : MonoBehaviour
{

    public Text[] remain;// remina Dirt, Root, Pebble;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRemainText(int remainDirt, int remainRoot, int remainPebble)
    {
        remain[0].text = remainDirt.ToString();
        remain[1].text = remainRoot.ToString();
        remain[2].text = remainPebble.ToString();
    }
}
