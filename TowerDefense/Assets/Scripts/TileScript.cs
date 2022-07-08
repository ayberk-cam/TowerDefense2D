using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TileScript : MonoBehaviour
{
    private Point GridPosition { get; set; }

    public Vector2 WorldPosition
    {
        get
        {
            var position = transform.position;
            return new Vector2(position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2),
                position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
    }

    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos,this);
    }
    
}
