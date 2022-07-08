using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform enemies;
    [SerializeField] private GameObject bar;
    private Waypoint _waypoints;
    private int _waypointIndex;

    private Stats _healthStats;

    public int Health
    {
        get => _healthStats.CurrentValue;
        set
        {
            _healthStats.ReduceBar(bar);
            _healthStats.CurrentValue = value;
        }
    }

    public bool isAlive;

    private void Awake()
    {
        _healthStats = new Stats();
        enemies = GameObject.FindGameObjectWithTag("Enemies").transform;
    }

    private void Start()
    {
        _waypoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoint>();
    }

    private void Update()
    {
        if (!GameManager.Instance.gameState) return;
        transform.position = Vector2.MoveTowards(transform.position, _waypoints.waypoints[_waypointIndex].position,
            speed * Time.deltaTime);
        if (!(Vector2.Distance(transform.position, _waypoints.waypoints[_waypointIndex].position) < 0.1f)) return;
        if (_waypointIndex < _waypoints.waypoints.Length - 1)
        {
            _waypointIndex++;
        }
        else
        {
            transform.GetComponent<EnemyScript>().isAlive = false;
            GameManager.Instance.Pool.ReleaseObject(gameObject);
            LevelManager.Instance.GameOverMenu();
        }
        if (gameObject.GetComponent<EnemyScript>().Health <= 0 )
        {
            GameManager.Instance.Currency += 1;
            GameManager.Instance.Score += 1;
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
        if (enemies.transform.childCount == 1)
        {
            GameManager.Instance.StartWave();
        }
    }

    public void Spawn()
    {
        transform.position = LevelManager.Instance.GreenPortal.transform.position;

        _healthStats.CurrentValue = 100;
        _healthStats.MaxValue = _healthStats.CurrentValue;
        
        StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1, 1)));
        
        transform.SetParent(enemies);
        transform.GetComponent<EnemyScript>().isAlive = true;
    }

    private IEnumerator Scale(Vector3 from, Vector3 to)
    {
        float progress = 0;
        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(from,to,progress);
            progress += Time.deltaTime;
            yield return null;
        }
        transform.localScale = to;
    }
}
