using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class Robot : MonoBehaviour
{

    public float chargePercentage
    {
        get => _chargePercentage;
        set
        {
            value = Mathf.Clamp01(value);
            _chargePercentage = value;
            ChargeChanged.Raise(_chargePercentage);
            if (_chargePercentage == 0f) PlayerDead.Raise();
        }
    }
    public GameEvent PlayerDead;
    public GameEventFloat ChargeChanged;
    public float chargingSpeed = 0.1f;
    public float drainingSpeed = 0.15f;
    private Collider sunCollider = null;
    [SerializeField] private float _chargePercentage = 1f;
    private Resource pickedUpResource = null;
    [SerializeField] Resource resourceInRange = null;

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
            sunCollider = other.GetComponent<Collider>();
        }
        else
        {
            Resource resource = other.GetComponent<Resource>();
            if (resource == null) return;
            if (resourceInRange != null) return;
            resourceInRange = resource;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Collider otherCollider = other.GetComponent<Collider>();
        if (other.tag.ToLower().Equals("sun")
            && otherCollider == sunCollider)
        {
            sunCollider = null;
        }
        else
        {
            Resource resource = other.GetComponent<Resource>();
            if (resource == null) return;
            if (resourceInRange == resource) resourceInRange = null;
        }
    }
}