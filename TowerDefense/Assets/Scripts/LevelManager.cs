using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject redPortalPrefab;
    [SerializeField] private GameObject greenPortalPrefab;
    [SerializeField] private Transform map;
    [SerializeField] private Transform towers;
    [SerializeField] private GameObject gameOverMenu;

    private float TileSize => tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;

    public Dictionary<Point, TileScript> Tiles;
    private List<Point> _possibleTowerPoints;
    private List<Tower> _existingTowersList;

    private Point _redPortalSpawn, _greenPortalSpawn;
    
    public Portal GreenPortal { get; private set; }
    private Portal RedPortal { get; set; }

    void Start()
    {
        CreateLevel();
    }
    
    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();
        _possibleTowerPoints = new List<Point>();
        _existingTowersList = new List<Tower>();
        
        var mapData = ReadLevelText();

        var mapX = mapData[0].ToCharArray().Length;
        var mapY = mapData.Length;

        if (Camera.main != null)
        {
            var worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        
            for (var y = 0; y < mapY; y++)
            {
                var newTiles = mapData[y].ToCharArray();
            
                for (var x = 0; x < mapX; x++)
                {
                    PlaceTile(newTiles[x].ToString(),x,y,worldStart);
                }
            }
        }
        SpawnPortals();
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        var tileIndex = int.Parse(tileType);
        if (tileIndex == 1)
        {
            _possibleTowerPoints.Add(new Point(x,y));
        }
        var newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();
        newTile.Setup(new Point(x,y),new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0),map);
    }

    private string[] ReadLevelText()
    {
        var tmpData = Resources.Load("Level") as TextAsset;
        if (tmpData != null)
        {
            var data = tmpData.text.Replace(Environment.NewLine, string.Empty);
            return data.Split('-');
        }
        else
        {
            return null;
        }
    }

    private void SpawnPortals()
    {
        var greenSpawn = new Point(0, 1);
        var tmpGreen = Instantiate(greenPortalPrefab, Tiles[greenSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        GreenPortal = tmpGreen.GetComponent<Portal>();
        GreenPortal.name = "GreenPortal";
        
        var redSpawn = new Point(8, 1);
        var tmpRed = Instantiate(redPortalPrefab, Tiles[redSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        RedPortal = tmpRed.GetComponent<Portal>();
        RedPortal.name = "RedPortal";
    }
    
    public void PlaceTower()
    {
        if (_possibleTowerPoints.Count == 0) return;
        if (GameManager.Instance.ClickedButton == null) return;
        if (GameManager.Instance.Currency < GameManager.Instance.ClickedButton.Currency) return;
        
        var randomIndex = new Random();
        var index = randomIndex.Next(_possibleTowerPoints.Count);
        
        var point = _possibleTowerPoints[index];
        var tile = LevelManager.Instance.Tiles[point];
        var position = tile.WorldPosition ;
        
        var towerObject = Instantiate(GameManager.Instance.ClickedButton.TowerPrefab, towers, false);
        towerObject.GetComponent<SpriteRenderer>().sortingOrder = point.GetY();
        towerObject.transform.position = position;
        
        var randomDamage = new Random();
        var damage = randomDamage.Next(0, 51);
        var currency = GameManager.Instance.ClickedButton.Currency;
        var tmpTower = new Tower(damage, point, currency, towerObject);
        
        _existingTowersList.Add(tmpTower);
        _possibleTowerPoints.RemoveAt(index);
        
        GameManager.Instance.Currency -= GameManager.Instance.ClickedButton.Currency;
    }

    public void DestroyTower()
    {
        if (_existingTowersList.Count == 0) return;
        if (GameManager.Instance.ClickedButton == null) return;
        
        var random = new Random();
        var index = random.Next(_existingTowersList.Count);
        
        GameManager.Instance.Currency += _existingTowersList[index].GetCurrency();
        var backPoint = _existingTowersList[index].GetGridPosition();
        
        Destroy(_existingTowersList[index].GetTower());
        _possibleTowerPoints.Add(backPoint);
        _existingTowersList.RemoveAt(index);
    }

    public void GameOverMenu()
    {
        GameManager.Instance.gameState = false;
        gameOverMenu.SetActive(true);
    }
}
