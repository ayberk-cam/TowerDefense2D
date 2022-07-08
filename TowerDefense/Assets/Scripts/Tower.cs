using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tower
{
      private GameObject _tower { get; set; }
      private int Damage { get; set; }
      private int Currency { get; set; }
      private Point GridPosition { get; set; }

      public Tower(int damage, Point gridPosition, int currency ,GameObject tower)
      {
            Damage = damage;
            GridPosition = gridPosition;
            _tower = tower;
            Currency = currency;
      }

      public int GetDamage()
      {
            return Damage;
      }

      public Point GetGridPosition()
      {
            return GridPosition;
      }

      public GameObject GetTower()
      {
            return _tower;
      }

      public int GetCurrency()
      {
            return Currency;
      }
}
