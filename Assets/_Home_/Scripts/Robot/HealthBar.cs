using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Gradient colorGradient;
    public Image fillingImage;
    private Canvas canvas;
    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }
    public void ChangeCharge(float newCharge)
    {
        if (newCharge >= 1f)
        {
            canvas.enabled = false;
            return;
        }
        canvas.enabled = true;
        fillingImage.fillAmount = newCharge;
        fillingImage.color = colorGradient.Evaluate(newCharge);
    }
}
