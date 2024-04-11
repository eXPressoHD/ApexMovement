using System;
using UnityEngine;

public class Gun : MonoBehaviour
{

    private float _damage = 10f;
    private float _range = 100f;
    private float _fireRate = 8f;

    [SerializeField]
    private ParticleSystem _muzzleFlash;

    [SerializeField]
    private Camera _fpsCam;

    [SerializeField]
    private AudioSource _shotSoundEffect;

    [SerializeField]
    public GameObject _impactEffect;

    [SerializeField]
    public Recoil _recoil;

    private float _nextTimeToFire = 0f;

    void Update()
    {
        if(Input.GetButton("Fire1") && Time.time >= _nextTimeToFire)
        {
            _nextTimeToFire = Time.time + 1f / _fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {        
        _shotSoundEffect.Play();
        _muzzleFlash.Play();
        _recoil.Fire();

        RaycastHit hit;
        if(Physics.Raycast(_fpsCam.transform.position, _fpsCam.transform.forward, out hit, _range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(_damage);
            }

            GameObject impactGo = Instantiate(_impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 2f);
        }
    }
}
