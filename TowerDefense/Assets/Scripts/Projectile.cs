using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private EnemyScript _target;
    private TowerScript _parent;
    
    [SerializeField] private int damage;
    private int Damage => damage;
    
    private void Update()
    {
        MoveToTarget();
    }

    public void Initialize(TowerScript parent)
    {
        this._target = parent.Target;
        this._parent = parent;
    }

    private void MoveToTarget()
    {
        if (_target != null && _target.isAlive)
        {
            var position = _target.transform.position;
            var position1 = transform.position;
            position1 = Vector3.MoveTowards(position1, position,
                Time.deltaTime * _parent.ProjectTileSpeed);
            transform.position = position1;
            Vector2 direction = position - position1;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (!_target.isAlive)
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy")) return;
        _target.Health -= Damage;
        GameManager.Instance.Pool.ReleaseObject(gameObject);
    }
}
