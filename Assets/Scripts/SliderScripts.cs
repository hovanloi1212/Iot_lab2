using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class SliderScripts : MonoBehaviour
{
	public bool b = true;
	//public Image image;
	public Slider fill;
	public float speed = 0.7f;
	public Text min_value;
	public Text max_value;
	public Text avg_value;
	float time = 0f;
	public Text progress;
	public Color less_than_avg = new Color32(2, 255, 132, 255);
	public Color more_than_avg = new Color32(250, 22, 42, 255);
	private void Start()
    {
		min_value.text = fill.minValue.ToString();
		max_value.text = fill.maxValue.ToString();
		float avg = (fill.minValue + fill.maxValue) / 2;
		avg_value.text = avg.ToString();
    }
    void Update()
	{
		if (b)
		{
			time += Time.deltaTime * speed;
			fill.value = time;
			if (time > 50)
            {
				max_value.color = more_than_avg;
				min_value.color = more_than_avg;
				min_value.color = more_than_avg;
			}
			else
            {
				max_value.color = less_than_avg;
				min_value.color = less_than_avg;
				min_value.color = less_than_avg;
			}
			if (time > fill.maxValue)
			{
				time = 0;
			}
		}
	}
}