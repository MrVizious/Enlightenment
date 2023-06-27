using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEvents;

public class GameManager : MonoBehaviour
{
    public Button buildCabinButton, buildFarmButton, buildInvestigationButton;
    public GameObject menuUI, gameUI;
    public GameEvent advancedToArriving;
    [System.Serializable]
    public enum GameState
    {
        Menu,
        Arriving,
        BuildCabin,
        BuildFarm,
        BuildInvestigation,
        ReceivingPackage,
        CarryPackage,
        LeavingPlanet,
        Lost
    }

    [SerializeField] private GameState _gameState = GameState.Menu;
    public GameState gameState
    {
        get => _gameState;
        set
        {
            if (value != gameState + 1 && value != gameState) return;
            // Change to invalid
            if (value == GameState.Menu)
            {
                menuUI?.SetActive(true);
                gameUI?.SetActive(false);
                buildCabinButton.gameObject.SetActive(false);
                buildFarmButton.gameObject.SetActive(false);
                buildInvestigationButton.gameObject.SetActive(false);
                _gameState = value;
            }
            if (value == GameState.Arriving)
            {
                menuUI?.SetActive(false);
                gameUI?.SetActive(true);
                buildCabinButton.gameObject.SetActive(false);
                buildFarmButton.gameObject.SetActive(false);
                buildInvestigationButton.gameObject.SetActive(false);
                _gameState = value;
                gameState++;
            }
            if (value == GameState.BuildCabin)
            {
                menuUI?.SetActive(false);
                gameUI?.SetActive(true);
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

    public void DropItemByRocket(Vector3 initialPosition, GameObject itemToDrop)
    {

    }


    private void Start()
    {
        gameState = gameState;
    }
}
