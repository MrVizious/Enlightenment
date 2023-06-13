using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UtilityMethods;

public class CameraManager : MonoBehaviour
{
    public enum CameraControlType
    {
        Simple,
        Advanced
    }
    public Transform cameraPivot;
    public Camera cam
    {
        get
        {
            if (_cam == null)
            {
                _cam = GetComponent<Camera>();
            }
            if (_cam == null)
            {
                _cam = Camera.main;
            }
            return _cam;
        }
    }

    public CameraControlType cameraControlType = CameraControlType.Advanced;
    public float minMovementSpeed = 30f, maxMovementSpeed = 60f;
    public float minFOV = 40f, maxFOV = 100f;
    public float zoomDampeningSpeed = 2f;
    public float zoomStep = 15f;
    public float findingSpeed = 100f;

    [ShowInInspector]
    public float desiredFOV
    {
        get => _desiredFOV;
        set
        {
            value = Mathf.Clamp(value, minFOV, maxFOV);
            _desiredFOV = value;
        }
    }
    private float _desiredFOV;
    [SerializeField]
    private Camera _cam = null;
    [SerializeField]
    private bool findingPlayer = false;
    private float movementSpeed
    {
        get
        {
            float percentageFOV = Math.Remap(cam.fieldOfView, minFOV, maxFOV, 0f, 1f);
            float s = Mathf.Lerp(minMovementSpeed, maxMovementSpeed, percentageFOV);
            return s;
        }
    }

    private Transform _player;
    private Transform player
    {
        get
        {
            if (_player == null)
            {
                _player = FindObjectOfType<Robot>().transform;
            }
            return _player;
        }
        set => _player = value;
    }
    private void Start()
    {
        desiredFOV = cam.fieldOfView;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) findingPlayer = !findingPlayer;
        if (!findingPlayer) MovementUpdate();
        else FindPlayerUpdate();

        ZoomUpdate();
    }

    private void MovementUpdate()
    {
        float verticalMovement = Input.GetAxis("Vertical");
        cameraPivot.Rotate(Vector3.right, movementSpeed * verticalMovement * Time.deltaTime);
        float rotationalMovement = 0f;
        if (cameraControlType == CameraControlType.Simple)
        {
            rotationalMovement = Input.GetAxis("Horizontal");
            cameraPivot.Rotate(Vector3.forward, -movementSpeed * 2f * rotationalMovement * Time.deltaTime);
        }
        else if (cameraControlType == CameraControlType.Advanced)
        {
            float horizontalMovement = Input.GetAxis("Horizontal");
            cameraPivot.Rotate(Vector3.up, -movementSpeed * horizontalMovement * Time.deltaTime);

            rotationalMovement = Input.GetAxis("Rotational");
            cameraPivot.Rotate(Vector3.forward, -movementSpeed * 2f * rotationalMovement * Time.deltaTime);
        }
    }

    private void ZoomUpdate()
    {
        if (Input.mouseScrollDelta.y > 0) ZoomIn();
        else if (Input.mouseScrollDelta.y < 0) ZoomOut();
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, desiredFOV, zoomDampeningSpeed * Time.deltaTime);
    }

    private void FindPlayerUpdate()
    {
        findingPlayer = true;
        Quaternion desiredDestination = Quaternion.LookRotation(-player.up, Vector3.up);
        cameraPivot.rotation = Quaternion.RotateTowards(cameraPivot.rotation, desiredDestination,
                                findingSpeed * Time.deltaTime);
        if (Quaternion.Angle(desiredDestination, cameraPivot.rotation) < 0.2f)
        {
            findingPlayer = false;
        }

    }
    public void ZoomIn()
    {
        desiredFOV -= zoomStep;
    }
    public void ZoomOut()
    {
        desiredFOV += zoomStep;
    }
}
