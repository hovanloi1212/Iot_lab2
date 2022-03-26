using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class Tgle : MonoBehaviour
{
    public GameObject switchBtn;
    public Image bkg;
    public bool isOn = false;
    private float speed = 0.4f;
    public Color off_color = new Color32(194, 54, 22, 255);
    public Color on_color = new Color32(76, 209, 55, 255);
    float x;
    public void Off_Switch()
    {
        switchBtn.transform.DOLocalMoveX(-x, speed);
        isOn = false;
        bkg.color = off_color;
    }

    public void On_Switch()
    {
        switchBtn.transform.DOLocalMoveX(x, speed);
        isOn = true;
        bkg.color = on_color;
    }
    public void OnSwitchButtonClicked()
    {
        if (switchBtn.transform.localPosition.x > 0)
        {
            Off_Switch();
        }
        else
        {
            On_Switch();
        }
    }
    private void Start()
    {
        x = (bkg.rectTransform.rect.width - switchBtn.GetComponent<RectTransform>().rect.width) / 2;
    }
}