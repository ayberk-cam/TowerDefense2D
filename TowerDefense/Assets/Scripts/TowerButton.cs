using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    public GameObject TowerPrefab => towerPrefab;
    
    [SerializeField] private int currency;
    public int Currency => currency;
}
