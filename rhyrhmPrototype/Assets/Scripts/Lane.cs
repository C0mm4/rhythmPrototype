using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Vector2 dir
    {
        get
        {
            return _dir;
        }
        set 
        {
            _dir = value;
        }
    }
    private Vector2 _dir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
