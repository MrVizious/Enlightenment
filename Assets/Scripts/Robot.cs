using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{

    public float chargePercentage
    {
        get => _chargePercentage;
        set
        {
            value = Mathf.Clamp01(value);
            //TODO: Has completely charged event
            _chargePercentage = value;
        }
    }
    public float chargingSpeed = 0.1f;
    public float drainingSpeed = 0.15f;
    private Collider sunCollider = null;
    [SerializeField] private float _chargePercentage = 1f;

    // Update is called once per frame
    void Update()
    {
        // If it is not in the sunlight
        if (sunCollider == null)
        {
            chargePercentage -= drainingSpeed * Time.deltaTime;
        }
        else
        {
            chargePercentage += chargingSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToLower().Equals("sun"))
        {
            Debug.Log("Is in the sunlight!");
            sunCollider = other.GetComponent<Collider>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Collider otherCollider = other.GetComponent<Collider>();
        if (other.tag.ToLower().Equals("sun")
            && otherCollider == sunCollider)
        {
            Debug.Log("Is out of the sunlight!");
            sunCollider = null;
        }
    }
}
