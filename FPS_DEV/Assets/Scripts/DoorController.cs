using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator _animator;
    CameraShake _camShake;

    private void Start()
    {
        _animator = gameObject.transform.parent.gameObject.transform.parent.GetComponent<Animator>();
        _camShake = Camera.main.GetComponent<CameraShake>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject player = other.gameObject;
            PlayerMovement pm = player.GetComponent<PlayerMovement>();

            if(pm.IsSprinting)
            {
                _animator.SetTrigger("DoorHit");
                _camShake.ShakeCamera();
            }
        }
    }
}
