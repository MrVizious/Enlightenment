using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceNeededUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text text;
    private string textPrefix = "x ";

    public void SetData(ResourceSO newData, int amountNeeded)
    {
        iconImage.sprite = newData.icon;
        SetAmountNeeded(amountNeeded);
    }

    public void SetAmountNeeded(int amountNeeded)
    {
        if (amountNeeded <= 0) Destroy(gameObject);
        text.text = textPrefix + amountNeeded;
    }
}
