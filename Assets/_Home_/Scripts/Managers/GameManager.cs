using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEvents;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    public Button buildCabinButton, buildFarmButton, buildInvestigationButton;
    public GameObject menuUI, gameUI;
    public GameEvent advancedToArriving, canMove, canNotMove;
    public RocketItemDropper rocketDropperPrefab;
    public GameObject finalResource;
    public GameObject robotAndTarget;
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
        GameOver,
        GameWon
    }

    [SerializeField] private GameState _gameState = GameState.Menu;
    public GameState gameState
    {
        get => _gameState;
        set
        {
            if (value == GameState.Menu)
            {
                canNotMove.Raise();
                menuUI?.SetActive(true);
                gameUI?.SetActive(false);
                FindObjectOfType<ResourceSpawner>()?.SpawnInitialResources();
                Structure[] structures = FindObjectsOfType<Structure>();
                foreach (Structure structure in structures)
                {
                    Destroy(structure.gameObject);
                }
                Destroy(GameObject.Find("Robot and Target Combo(Clone)"));
                Transform copy = GameObject.Find("Main Camera Pivot Copy").transform;
                GameObject.Find("Main Camera Pivot").transform.SetPositionAndRotation(
                    copy.position, copy.rotation
                );
                _gameState = value;
            }
            if (value == GameState.Arriving)
            {
                canNotMove.Raise();
                DropRobot().Forget();
                buildCabinButton.gameObject.SetActive(false);
                buildFarmButton.gameObject.SetActive(false);
                buildInvestigationButton.gameObject.SetActive(false);
                _gameState = value;
            }
            if (value == GameState.BuildCabin)
            {
                canMove.Raise();
                gameUI?.SetActive(true);
                FindObjectOfType<CameraManager>()?.StartFindingPlayer();
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
                DropFinalResource().Forget();
                _gameState = value;
            }
            else if (value == GameState.GameWon)
            {
                Debug.Log("Game Won!");
                _gameState = value;
            }
            else if (value == GameState.GameOver)
            {
                if (gameState >= GameState.BuildCabin
                   && gameState <= GameState.CarryPackage)
                {
                    Debug.Log("Game Over!");
                    _gameState = value;
                }
            }
            else
            {
                _gameState = value;
            }

        }
    }

    private Quaternion rotationDifference;

    private void Start()
    {
        Transform introPivot = GameObject.Find("Intro Camera Pivot").transform;
        Transform mainPivot = GameObject.Find("Main Camera Pivot").transform;
        rotationDifference = Quaternion.RotateTowards(introPivot.transform.rotation, mainPivot.transform.rotation, Quaternion.Angle(introPivot.transform.rotation, mainPivot.transform.rotation));
        gameState = gameState;
    }

    public async UniTask DropItemByRocket(Vector3 initialPosition, Vector3 moonPosition, GameObject itemToDrop)
    {

        Ray ray = new Ray(initialPosition, moonPosition - initialPosition);
        RaycastHit hit;

        // Figure out where the ground is
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~LayerMask.NameToLayer("Ground")))
        {
            Vector3 p = hit.point;
            Vector3 normal = hit.normal;
            Vector3 forward = Vector3.Cross(Random.insideUnitSphere, normal).normalized;
            var rot = Quaternion.LookRotation(forward, normal);
            RocketItemDropper rocket = Instantiate(rocketDropperPrefab, initialPosition, rot);
            await rocket.InitializeSequence(initialPosition, p, rot, itemToDrop);
        }
    }

    [Button]
    public async UniTask DropRobot()
    {
        Vector3 moonPosition = GameObject.Find("Moon").transform.position;
        Vector3 initialPosition = GameObject.Find("Rocket Drop Point").transform.position;
        await DropItemByRocket(Camera.main.transform.position, moonPosition, robotAndTarget);
        gameState = GameState.BuildCabin;
    }

    [Button]
    public async UniTask DropFinalResource()
    {
        Vector3 investigationCenter = GameObject.Find("Investigation(Clone)").transform.position;
        Vector3 moonPosition = GameObject.Find("Moon").transform.position;
        Vector3 initialPosition = moonPosition - (investigationCenter - moonPosition) * 2;

        await DropItemByRocket(initialPosition, moonPosition, finalResource);
        gameState = GameState.CarryPackage;
    }



    [Button]
    public void SetGameState(GameState newState)
    {
        gameState = newState;
    }
}
