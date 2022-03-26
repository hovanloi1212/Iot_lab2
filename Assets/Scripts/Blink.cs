using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Blink : MonoBehaviour
{
    // Start is called before the first frame update
    public Image image;
    // Update is called once per frame
    public bool blink = true;
    float time = 0f;
    public float speed = 0.1f;
    void Update()
    {
        if (blink)
        {
            Color color = image.color;
            time += Time.deltaTime * speed;
            color.a = time;
            image.color = color;
            if (time > 1) time = 0;
        }
    }
}
