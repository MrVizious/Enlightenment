using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalResource : Resource
{
    public float secondsToExplode;
    private float secondsBeingCarriedInSun = 0f;
    private Canvas _canvas;
    private Canvas canvas
    {
        get
        {
            if (_canvas == null) _canvas = GetComponentInChildren<Canvas>();
            return _canvas;
        }
    }

    private Collider sunCollider = null;


    private void Update()
    {
        if (sunCollider != null)
        {
            ExplodingUpdate();
        }
        else
        {
            canvas.gameObject.SetActive(false);
        }
    }
    private void ExplodingUpdate()
    {
        if (isBeingCarried)
        {
            canvas.gameObject.SetActive(true);
            secondsBeingCarriedInSun += Time.deltaTime;
            if (secondsBeingCarriedInSun >= secondsToExplode)
            {
                Explode();
            }
        }
        else canvas.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.ToLower().Equals("sun")) return;
        if (sunCollider != null) return;
        sunCollider = other;
    }
    private void OnTriggerExit(Collider other)
    {
        if (sunCollider == null) return;
        if (other != sunCollider) return;
        sunCollider = null;
        secondsBeingCarriedInSun = 0f;
    }
    private void Explode()
    {
        Debug.Log("BOOOOOM!!");
    }
}
