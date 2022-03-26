using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class Temperature : MonoBehaviour
{
    // Start is called before the first frame update
    public Text temperature;
    public Image image;
    float deg_cool = 30;
    float deg_warm = 51;
    public Color cool = new Color32(68, 189, 50, 255);
    public Color warm = new Color32(251, 197, 49, 255);
    public Color hot = new Color32(232, 65, 24, 255);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float deg = (float)float.Parse(Regex.Match(temperature.text, @"([-+]?[0-9]*\.?[0-9]+)").Value);
        if (deg <= deg_cool)
        {
            image.color = cool;
            temperature.color = cool;
        }
        else if (deg < deg_warm)
        {
            image.color = warm;
            temperature.color = warm;
        }
        else
        {
            image.color = hot;
            temperature.color = hot;
        }
    }
}
