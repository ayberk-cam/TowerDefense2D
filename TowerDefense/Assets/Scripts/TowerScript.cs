using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    [SerializeField] private string projectTileType;
    [SerializeField] private float projectTileSpeed;
    [SerializeField] private float attackCooldown;

    private readonly Queue<EnemyScript> _enemies = new Queue<EnemyScript>();
    private EnemyScript _target;
    private bool _canAttack = true;
    private float _attackTimer;

    public float ProjectTileSpeed => projectTileSpeed;
    public EnemyScript Target => _target;

    void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (!_canAttack)
        {
            _attackTimer += Time.deltaTime;
            
            if (_attackTimer >= attackCooldown)
            {
                _canAttack = true;
                _attackTimer = 0;
            }
        }
        if (_target == null && _enemies.Count > 0)
        {
            _target = _enemies.Dequeue();
        }

        if (_target != null && _target.isAlive && _target.Health > 0)
        {
            if (_canAttack)
            {
                Shoot();
                _canAttack = false;
            }
        }
    }

    private void Shoot()
    {
        var projectile = GameManager.Instance.Pool.GetObject(projectTileType).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.Initialize(this);
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            _enemies.Enqueue(col.GetComponent<EnemyScript>());
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            _target = null;
        }
    }
    
}
