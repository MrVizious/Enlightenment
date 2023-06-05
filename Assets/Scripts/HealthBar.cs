using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Robot robot;
    public Gradient colorGradient;
    private Image fillingImage;
    private void Start()
    {
        fillingImage = GetComponent<Image>();
    }
    void Update()
    {
        fillingImage.fillAmount = robot.chargePercentage;
        fillingImage.color = colorGradient.Evaluate(robot.chargePercentage);
    }
}
