using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class Humidity : MonoBehaviour
{
    // Start is called before the first frame update
    public Text humidity;
    public Image image;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int deg = (int)int.Parse(Regex.Match(humidity.text, @"\d+").Value);
        if (deg < 40)
        { 
            image.color = new Color32(0, 168, 255, 255); 
            humidity.color = new Color32(0, 168, 255, 255);
        }
        else if (deg < 71) 
        {
            image.color = new Color32(0, 151, 230, 255);
            humidity.color = new Color32(0, 151, 230, 255);
        }
        else if (deg< 81)
        {
            image.color = new Color32(72, 126, 176, 255);
            humidity.color = new Color32(72, 126, 176, 255);
        }
        else
        { 
            image.color = new Color32(64, 115, 158, 255);
            humidity.color = new Color32(64, 115, 158, 255);
        }
    }
}
