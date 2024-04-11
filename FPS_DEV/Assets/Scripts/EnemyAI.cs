using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent;
    private Transform _player;
    [SerializeField]
    private LayerMask _groundMask, _playerMask;

    private Animator _anim;

    [SerializeField]
    private float _lookRotationYDamping;

    //Patrol
    private Vector3 _walkPoint;
    private bool _walkPointSet;
    [SerializeField]
    private float _walkPointRange;    

    //Attacking
    [SerializeField]
    private float _timeBetweenAttacks;
    private bool _alreadyAttacked;

    //States
    [SerializeField]
    private float _sightRange, _attackRange;
    [SerializeField]
    private bool _playerInSightRange, _playerInAttackRange;

    private void Awake()
    {
        _player = GameObject.Find("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(_agent.velocity.magnitude > 0f)
        {
            _anim.SetFloat("VerticalInput", 1f);
        }
        
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _playerMask);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _playerMask);

        if (!_playerInSightRange && !_playerInAttackRange)
        {
            _agent.isStopped = false;
            Patrolling();
        }

        if (_playerInSightRange && !_playerInAttackRange)
        {
            _agent.isStopped = false;
            ChasePlayer();
        }

        if (_playerInSightRange && _playerInAttackRange)
        {
            _agent.isStopped = true;
            AttackPlayer();
        }

        if(_agent.isStopped)
        {
            _anim.SetFloat("VerticalInput", 0f);
            _anim.SetFloat("HorizontalInput", 0f);
        }
    }

    private void Patrolling()
    {
        if (!_walkPointSet)
        {
            SearchWalkPoint();
        }

        if (_walkPointSet)
        {
            _agent.SetDestination(_walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - _walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            _walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomX = UnityEngine.Random.Range(-_walkPointRange, _walkPointRange);
        float randomZ = UnityEngine.Random.Range(-_walkPointRange, _walkPointRange);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_walkPoint, -transform.up, 2f, _groundMask))
        {
            _walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_player.position);

        Vector3 distanceToPlayer = transform.position - _player.position;

        if (distanceToPlayer.magnitude <= 3f)
        {
            _playerInAttackRange = true;
        }
        else
        {
            _playerInAttackRange = false;
        }
    }

    private void AttackPlayer()
    {
        _agent.isStopped = true;

        //Lookat
        var lookPos = _player.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1f);

        if (!_alreadyAttacked)
        {
            //Attack here, shooting...

            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }
}
