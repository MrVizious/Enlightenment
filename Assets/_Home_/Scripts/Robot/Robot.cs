using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class Robot : MonoBehaviour
{
    public bool isInLight => sunCollider != null;
    public float chargePercentage
    {
        get => _chargePercentage;
        set
        {
            value = Mathf.Clamp01(value);
            _chargePercentage = value;
            chargeChanged.Raise(_chargePercentage);
            if (_chargePercentage == 0f
                && gameManager.gameState >= GameManager.GameState.BuildCabin
                && gameManager.gameState <= GameManager.GameState.CarryPackage
            )
            {
                gameLost.Raise();
            }
        }
    }
    public GameEvent gameLost;
    public GameEventFloat chargeChanged;
    public float chargingSpeed = 0.1f;
    public float drainingSpeed = 0.15f;
    [SerializeField] private Collider sunCollider = null;
    [SerializeField] private float _chargePercentage = 1f;
    [SerializeField] private Light _nightLight;
    private Light nightLight
    {
        get
        {
            if (_nightLight == null) _nightLight = GetComponentInChildren<Light>();
            return _nightLight;
        }
    }
    private GameManager _gameManager;
    private GameManager gameManager
    {
        get
        {
            if (_gameManager == null) _gameManager = FindObjectOfType<GameManager>();
            return _gameManager;
        }
    }

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
            if (sunCollider != null) return;

            Collider otherCollider = other.GetComponent<Collider>();

            if (otherCollider == null) return;

            sunCollider = other;
            nightLight.enabled = false;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag.ToLower().Equals("sun"))
        {
            if (sunCollider == null) return;

            Collider otherCollider = other.GetComponent<Collider>();

            if (otherCollider != sunCollider) return;

            sunCollider = null;
            nightLight.enabled = true;
        }
    }
}