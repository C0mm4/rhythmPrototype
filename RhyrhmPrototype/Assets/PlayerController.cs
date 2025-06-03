using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public TestCode code;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseMove(InputValue value)
    {
        Vector2 mousePos = value.Get<Vector2>();
        float time = Time.time;
    }

    public void OnKey(InputAction.CallbackContext ctx)
    {
        var keyPressed = Keyboard.current.allKeys
            .FirstOrDefault(k => k.wasPressedThisFrame);
        
        if (keyPressed != null)
        {
            double time = ctx.time;

            Debug.Log("Pressed key: " + keyPressed.keyCode + " Time : " + time + " " + code.OnInput(time));
        }
    }
}
