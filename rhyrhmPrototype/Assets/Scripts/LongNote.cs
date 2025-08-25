using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : Note
{
    double endTime;

    // Start is called before the first frame update
    override public void Start()
    {
        
    }

    public override void SetData(double dspTime, double startT, float radianDir, int index)
    {
        base.SetData(dspTime, startT, radianDir, index);
    }
}
