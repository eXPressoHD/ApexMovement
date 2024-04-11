using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    [SerializeField]
    private float _health = 100f;
    [SerializeField]
    private Slider _healthSlider;

    [SerializeField]
    private MeshDestroy _destroyScript;


    private void Start()
    {
        if (_healthSlider != null)
        {
            _healthSlider.value = _health;
        }
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;

        if (_healthSlider != null)
        {
            _healthSlider.value = _health;
        }

        if (_health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if(gameObject.tag == "Enemy")
        {
            GameObject parent = gameObject.transform.parent.gameObject;
            GameObject go = gameObject.transform.parent.GetChild(0).gameObject;
            go.SetActive(false);
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            _destroyScript.DestroyMesh();
            Destroy(parent);
        }

        _destroyScript.DestroyMesh();
    }
}
