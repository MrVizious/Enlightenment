using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button buildCabinButton, buildFarmButton, buildInvestigationButton;
    public enum GameState
    {
        Arriving,
        BuildCabin,
        BuildFarm,
        BuildInvestigation,
        ReceivingPackage,
        CarryPackage,
        LeavingPlanet,
        Lost
    }

    private GameState _gameState = GameState.Arriving;
    public GameState gameState
    {
        get => _gameState;
        set
        {
            if (value != gameState + 1) return;
            // Change to invalid
            if (value == GameState.BuildCabin)
            {
                buildCabinButton.gameObject.SetActive(true);
                _gameState = value;
            }
            else if (value == GameState.BuildFarm)
            {
                buildCabinButton.gameObject.SetActive(false);
                buildFarmButton.gameObject.SetActive(true);
                _gameState = value;
            }
            else if (value == GameState.BuildInvestigation)
            {
                buildFarmButton.gameObject.SetActive(false);
                buildInvestigationButton.gameObject.SetActive(true);
                _gameState = value;
            }
            else if (value == GameState.ReceivingPackage)
            {
                Debug.Log("Receiving package!");
                _gameState = value;
            }
        }
    }

    private void Start()
    {
        gameState = GameState.BuildCabin;
    }
}
