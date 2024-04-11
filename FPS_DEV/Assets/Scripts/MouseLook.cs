using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private float _mouseSensitivity = 100f;
    [SerializeField]
    private Transform _playerBody;

    private GameObject _player;
    private PlayerMovement _playerMovement;
    private float _desiredX;

    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _player = GameObject.Find("Player");
        _playerMovement = _player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        Vector3 rot = Camera.main.transform.localRotation.eulerAngles;
        _desiredX = rot.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, _playerMovement._wallRunCameraTilt);
        _playerMovement._orientation.transform.localRotation = Quaternion.Euler(0, _desiredX, 0);
        _playerBody.Rotate(Vector3.up * mouseX);

        //While Wallrunning
        //Tilts camera in .5 second
        if (Mathf.Abs(_playerMovement._wallRunCameraTilt) < _playerMovement._maxWallRunCameraTilt && _playerMovement._isWallRunning && _playerMovement._isWallRight)
            _playerMovement._wallRunCameraTilt += Time.deltaTime * _playerMovement._maxWallRunCameraTilt * 2;
        if (Mathf.Abs(_playerMovement._wallRunCameraTilt) < _playerMovement._maxWallRunCameraTilt && _playerMovement._isWallRunning && _playerMovement._isWallLeft)
            _playerMovement._wallRunCameraTilt -= Time.deltaTime * _playerMovement._maxWallRunCameraTilt * 2;

        //Tilts camera back again
        if (_playerMovement._wallRunCameraTilt > 0 && !_playerMovement._isWallRight && !_playerMovement._isWallLeft)
            _playerMovement._wallRunCameraTilt -= Time.deltaTime * _playerMovement._maxWallRunCameraTilt * 2;

        if (_playerMovement._wallRunCameraTilt < 0 && !_playerMovement._isWallRight && !_playerMovement._isWallLeft)
            _playerMovement._wallRunCameraTilt += Time.deltaTime * _playerMovement._maxWallRunCameraTilt * 2;

    }
}
