using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField]
    private Slider _healthbar;

    [SerializeField]
    private float _playerHealth;

    private void Start()
    {
        if (_healthbar != null)
        {
            _healthbar.value = _playerHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        _playerHealth -= amount;

        if (_healthbar != null)
        {
            _healthbar.value = _playerHealth;
        }

        if (_playerHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
