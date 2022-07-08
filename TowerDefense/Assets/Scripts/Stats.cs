using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Stats
{
    public int CurrentValue;
    public int MaxValue;

    public void Initialize()
    {
        this.CurrentValue = 100;
        this.MaxValue = 100;
    }

    public void ReduceBar(GameObject bar)
    {
        var barValue = (float)CurrentValue / MaxValue;
        bar.transform.localScale = new Vector3(barValue, 1);
    }
}
