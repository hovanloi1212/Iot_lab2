using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using System.Text.RegularExpressions;
public class Graph : MonoBehaviour
{
    private RectTransform graph_container;
    public bool b = false;
    [SerializeField]
    private Sprite dot_graph;
    [SerializeField]
    private Sprite line_graph;
    public Color color;
    public float sample_time = 15f;
    public float safe_change;
    [SerializeField]
    private Text Temperature;
    private float cd = 0;
    private List<float> value = new List<float>();
    GameObject lastGameObject = null;
    // Start is called before the first frame update
    void Start()
    {
        graph_container = transform.Find("Graphcontainer").GetComponent<RectTransform>();
    }
    private GameObject Create_circle(Vector2 position)
    {
        GameObject circle = new GameObject("circle", typeof(Image));
        circle.transform.SetParent(graph_container, false);
        circle.GetComponent<Image>().sprite = dot_graph;
        circle.GetComponent<Image>().color = color;
        RectTransform rectTransform = circle.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(30, 30);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return circle;
    }
    private void draw_graph(float x, float y)
    {
        GameObject create_circle = Create_circle(new Vector2(x, y));
        if (lastGameObject != null)
        {
            dotConnection(lastGameObject.GetComponent<RectTransform>().anchoredPosition,
                          create_circle.GetComponent<RectTransform>().anchoredPosition);
        }
        lastGameObject = create_circle;
    }
    private void show_Graph(float temp) 
    {
        float xSize = graph_container.sizeDelta.x / 6;
        float yMaximum = 100f;
        float GraphHeight = graph_container.sizeDelta.y;
        float x, y;
        if (value.Count == 7)
        {
            value.RemoveAt(0);
            foreach(Transform child in graph_container.transform)
            {
                Destroy(child.gameObject);
                lastGameObject = null;
            }
            for (int i = 0; i < value.Count;i++)
            {
                x = i * xSize;
                if (value[i] > yMaximum) y = GraphHeight;
                else y = (value[i] / yMaximum) * GraphHeight;
                draw_graph(x, y);
            }
        }
        value.Add(temp);
        x = (value.Count-1) * xSize;
        if (temp > yMaximum) y = GraphHeight;
        else y = (temp / yMaximum) * GraphHeight;
        draw_graph(x, y);

    }

    private void dotConnection(Vector2 dotA, Vector2 dotB)
    {
        GameObject line = new GameObject("line", typeof(Image));
        line.transform.SetParent(graph_container, false);
        line.GetComponent<Image>().sprite = line_graph;
        Color newcolor = color;
        //newcolor.a = 0.7f;
        line.GetComponent<Image>().color = newcolor;
        RectTransform rectTransform = line.GetComponent<RectTransform>();
        Vector2 dir = (dotB - dotA).normalized;
        float distance = Vector2.Distance(dotA, dotB);
        rectTransform.anchoredPosition = dotA + dir * distance * 0.5f;
        rectTransform.sizeDelta = new Vector2(distance, 5f);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }
    // Update is called once per frame
    void Update()
    {
        if (b)
        {
            float deg = (float)float.Parse(Regex.Match(Temperature.text, @"([-+]?[0-9]*\.?[0-9]+)").Value);
            float z = 0;
            if (value.Count > 0) z = Mathf.Abs(deg - value[value.Count - 1]);
            if (z <= safe_change)
            {
                if (cd < sample_time)
                {
                    cd += Time.deltaTime;
                }
                else
                {
                    cd = 0;
                    show_Graph(deg);
                }
            }
            else
            {
                cd = 0;
                show_Graph(deg);
            }
        }
    }
}
