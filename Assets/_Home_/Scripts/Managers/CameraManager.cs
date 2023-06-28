using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UtilityMethods;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public enum CameraControlType
    {
        Simple,
        Advanced
    }
    public Transform cameraPivot;
    public CinemachineVirtualCamera vCam;
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
    private bool findingPlayer = false, followingPlayer = false;
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
                _player = FindObjectOfType<Robot>()?.transform;
            }
            return _player;
        }
        set => _player = value;
    }
    private float verticalMovement = 0f, rotationalMovement = 0f, horizontalMovement = 0f;
    private void Start()
    {
        desiredFOV = vCam.m_Lens.FieldOfView;
    }
    private void Update()
    {
        InputUpdate();
        if (horizontalMovement != 0f || verticalMovement != 0f || rotationalMovement != 0f) MovementUpdate();
        if (followingPlayer)
        {
            if (findingPlayer) FindPlayerUpdate();
            else FollowPlayerUpdate();
        }
        else if (findingPlayer) FindPlayerUpdate();

        ZoomUpdate();
    }

    private void InputUpdate()
    {
        // Find player
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartFindingPlayer();
        }
        // Follow player
        else if (Input.GetKeyDown(KeyCode.X))
        {
            StartFollowingPlayer();
        }

        if (Input.mouseScrollDelta.y > 0) ZoomIn();
        else if (Input.mouseScrollDelta.y < 0) ZoomOut();

        verticalMovement = Input.GetAxis("Vertical");
        rotationalMovement = Input.GetAxis("Rotational");
        horizontalMovement = Input.GetAxis("Horizontal");
    }

    public void StartFindingPlayer()
    {
        if (followingPlayer) return;
        if (player == null) return;
        findingPlayer = !findingPlayer;
    }
    public void StartFollowingPlayer()
    {
        followingPlayer = !followingPlayer;
        findingPlayer = followingPlayer;
    }

    private void MovementUpdate()
    {
        findingPlayer = followingPlayer = false;
        cameraPivot.Rotate(Vector3.right, movementSpeed * verticalMovement * Time.deltaTime);
        if (cameraControlType == CameraControlType.Simple)
        {
            rotationalMovement = horizontalMovement;
            cameraPivot.Rotate(Vector3.forward, -movementSpeed * 2f * rotationalMovement * Time.deltaTime);
        }
        else if (cameraControlType == CameraControlType.Advanced)
        {
            cameraPivot.Rotate(Vector3.up, -movementSpeed * horizontalMovement * Time.deltaTime);
            cameraPivot.Rotate(Vector3.forward, -movementSpeed * 2f * rotationalMovement * Time.deltaTime);
        }
    }

    private void ZoomUpdate()
    {
        vCam.m_Lens.FieldOfView = Mathf.Lerp(cam.fieldOfView, desiredFOV, zoomDampeningSpeed * Time.deltaTime);
    }

    private void FindPlayerUpdate()
    {
        findingPlayer = true;
        Quaternion desiredRotation = Quaternion.LookRotation(-player.up, Vector3.up);
        cameraPivot.rotation = Quaternion.RotateTowards(cameraPivot.rotation, desiredRotation,
                                findingSpeed * Time.deltaTime);
        if (Quaternion.Angle(desiredRotation, cameraPivot.rotation) < 0.2f)
        {
            findingPlayer = false;
        }
    }

    private void FollowPlayerUpdate()
    {
        if (player == null) return;
        Quaternion desiredRotation = Quaternion.LookRotation(-player.up, Vector3.up);
        cameraPivot.rotation = Quaternion.Slerp(cameraPivot.rotation, desiredRotation, 0.02f);
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
