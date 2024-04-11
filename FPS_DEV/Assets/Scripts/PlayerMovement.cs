using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController _controller;

    [SerializeField]
    private Vector3 _velocity;

    [SerializeField]
    private float _playerSpeed = 8f;
    [SerializeField]
    private float _playerSprintSpeed = 12f;

    [SerializeField]
    private float _gravity = -19.62f;
    [SerializeField]
    private float _jumpHeight = 3f;

    private float _defaultGravity;

    [SerializeField]
    private Transform _groundCheck;
    [SerializeField]
    private float _groundDistance;
    [SerializeField]
    private LayerMask _groundMask;
    [SerializeField]
    private LayerMask _wallRunMask;

    [SerializeField]
    private float _wallRunForce, _maxWallRunTime, _maxWallRunSpeed;
    public bool _isWallRight, _isWallLeft, _isWallRunning;

    [SerializeField]
    public float _maxWallRunCameraTilt, _wallRunCameraTilt;

    [SerializeField]
    public Transform _orientation;

    [SerializeField]
    private bool _isGrounded, _isCrouching, _isSprinting;

    public bool IsSprinting
    {
        get
        {
            return _isSprinting;
        }
    }

    public CharacterController CharacterController
    {
        get
        {
            return _controller;
        }
    }

    private void Start()
    {        
        _isCrouching = false;
        _defaultGravity = _gravity;
    }

    void Update()
    {
        _isSprinting = false;
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        _controller.Move(move * _playerSpeed * Time.deltaTime);

        if(Input.GetKey(KeyCode.LeftShift))
        {
            _isSprinting = true;
            _controller.Move(move * _playerSprintSpeed * Time.deltaTime);
        }

        CheckForWall();
        WallRunInput();
        CrouchCheck();


        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _defaultGravity);
        }
        else if (Input.GetButtonDown("Jump") && _isWallRunning)
        {
            if (_isWallRight)
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -3f * _defaultGravity);
                _velocity.x = Mathf.Sqrt(_jumpHeight * -2f * _defaultGravity);
            }
            else if (_isWallLeft)
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -3f * _defaultGravity);
                _velocity.x = Mathf.Sqrt(_jumpHeight * 2f * _defaultGravity);
            }
        }

        _velocity.y += _defaultGravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
        _velocity.x = 0;
    }

    private void CrouchCheck()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _isCrouching = true;
            _controller.height = 1.6f;
            _playerSpeed = 6f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _isCrouching = false;
            _controller.height = 3.4f;
            _playerSpeed = 12f;
        }
    }

    private void WallRunInput()
    {
        if (Input.GetKey(KeyCode.D) && _isWallRight)
        {
            StartWallRun();
        }

        if (Input.GetKey(KeyCode.A) && _isWallLeft)
        {
            StartWallRun();
        }
    }

    private void StartWallRun()
    {
        _gravity = 100f;
        _isWallRunning = true;

        if (_controller.velocity.magnitude <= _maxWallRunSpeed)
        {
            _velocity.y = 0;
            _controller.Move(_orientation.forward * _wallRunForce / 5 * Time.deltaTime);

            if (_isWallRight)
            {
                _velocity.y = 0;
                _controller.Move(_orientation.right * _wallRunForce / 5 * Time.deltaTime);
            }
            else
            {
                _velocity.y = 0;
                _controller.Move(-_orientation.right * _wallRunForce / 5 * Time.deltaTime);
            }

            StartCoroutine(StartWallRunCountDown(1f));
        }
    }

    private IEnumerator StartWallRunCountDown(float maxWallRunTime)
    {
        yield return new WaitForSeconds(maxWallRunTime);

        if (_isWallRunning)
        {
            StopWallRun();
            _velocity.y = -2f;
            _controller.Move(_velocity * Time.deltaTime); //Reset character to floor
            _isWallRight = false;
            _isWallLeft = false;
        }

        yield return null;
    }

    private void StopWallRun()
    {
        _isWallRunning = false;
        _gravity = _defaultGravity;
    }

    private void CheckForWall()
    {
        _isWallRight = Physics.Raycast(transform.position, _orientation.right, 1f, _wallRunMask);
        _isWallLeft = Physics.Raycast(transform.position, -_orientation.right, 1f, _wallRunMask);

        if (!_isWallRight && !_isWallLeft) StopWallRun();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "DeadZone")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
