using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_UI : MonoBehaviour
{
    public Image currLevelImage;
    public Image overLevelImage;
    
    public void Init()
    {
        currLevelImage.fillAmount = 0f;
        overLevelImage.fillAmount = 0f;
    }

    public void SetLevel(int currLevel)
    {
        currLevelImage.fillAmount = ((float)currLevel + 1f)/5f;
        overLevelImage.fillAmount = (float)currLevel/ 5f;
    }
}
