using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniper : MonoBehaviour
{
    private AudioSource _sniperShotSound;
    private GameObject _player;
    private LineRenderer _line;
    [SerializeField]
    private Transform _sniperPosition;

    [SerializeField]
    private float _lookAtDelay;
    private float _fireRate;
    private float _nextFire;

    private void Start()
    {
        _line = GetComponent<LineRenderer>();
        _sniperShotSound = GetComponent<AudioSource>();
        _player = GameObject.Find("Player");
        _fireRate = 5f;
        _nextFire = Time.time;
    }

    private void Update()
    {
        CheckIfTimeToFire();
        var rotation = Quaternion.LookRotation(_player.transform.position - transform.position);
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _lookAtDelay);
        _sniperPosition.LookAt(_player.transform.position);
    }

    private void CheckIfTimeToFire()
    {
        if (Time.time > _nextFire)
        {
            _line.enabled = true;
            _line.SetPosition(0, _sniperPosition.position);
            RaycastHit hit;
            if (Physics.Raycast(_sniperPosition.position, _sniperPosition.forward, out hit, 150f)) //Range 100f
            {
                if (hit.collider)
                {
                    //If player hitted draw to point
                    _line.SetPosition(1, hit.point);
                    PlayerHealthController healthController = hit.transform.GetComponent<PlayerHealthController>();
                    if (healthController != null)
                    {
                        healthController.TakeDamage(30f);
                    }
                }
                else
                {
                    //else cut line at hitting object, so line wont go through
                    _line.SetPosition(1, transform.position + transform.forward * 100.0f);
                }

                _sniperShotSound.Play();                
            }
            _nextFire = Time.time + _fireRate;
            Debug.Log("Sniper has shot");
            StartCoroutine(FadeOutSniperLine());
        }
    }

    private IEnumerator FadeOutSniperLine()
    {
        yield return new WaitForSeconds(1.6f);
        _line.enabled = false;
    }
}
