using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UtilityMethods;

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
        float modifiedPercentage = Math.Remap(newCharge, 0.05f, 1f, 0f, 1f);
        fillingImage.fillAmount = modifiedPercentage;
        fillingImage.color = colorGradient.Evaluate(modifiedPercentage);
    }
}
