using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class GameManager : Singleton<GameManager>
{
    public TowerButton ClickedButton { get; set; }
    public bool gameState = true;

    private int _curreny;
    private int _waveNumber;
    private int _score;

    [SerializeField] private Text currencyText;
    [SerializeField] private Text waveText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text gameOverScoreText;
    
    public ObjectPool Pool { get; set; }

    public int Currency
    {
        get => _curreny;
        set
        {
            _curreny = value;
            currencyText.text = "Currency: " +  value.ToString() + "$";
        }
    }
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            scoreText.text = "Score: " +  value.ToString();
            gameOverScoreText.text = "Score: " +  value.ToString();
        }
    }
    
    public int WaveNumber
    {
        get => _waveNumber;
        set
        {
            _waveNumber = value;
            waveText.text = "Wave: " +  (value - 1).ToString();
        }
    }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }

    private void Start()
    {
        Currency = 5;
        WaveNumber = 1;
        Score = 0;
        StartWave();
    }
    
    public void PickTowerButton(TowerButton towerButton)
    {
        ClickedButton = towerButton;
        LevelManager.Instance.PlaceTower();
    }

    public void PickDestroyButton()
    {
        LevelManager.Instance.DestroyTower();
    }

    public void StartWave()
    {
        var num = WaveNumber;
        StartCoroutine(SpawnWave(num));
        WaveNumber += 1;
    }

    private IEnumerator SpawnWave(int num)
    {
        yield return new WaitForSeconds(1f);
        num = num * 2;
        var random = new Random();
        var index = random.Next(0, 3);

        var type = string.Empty;

        switch (index)
        {
            case 0:
                type = "Enemy_1";
                break;
            case 1:
                type = "Enemy_2";
                break;
            case 2:
                type = "Enemy_3";
                break;
        }

        for (var i = 0; i < num; i++)
        {
            var enemyScript =  Pool.GetObject(type).GetComponent<EnemyScript>();
            enemyScript.Spawn();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
