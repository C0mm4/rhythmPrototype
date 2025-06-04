using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public TestCode code;
    private Quaternion targetQuaternion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    public void OnKey(InputAction.CallbackContext ctx)
    {
        var keyPressed = Keyboard.current.allKeys
            .FirstOrDefault(k => k.wasPressedThisFrame);
        
        if (keyPressed != null)
        {
            code.OnInput();
        }
    }

    public void OnMouseMove(InputAction.CallbackContext ctx)
    {
        Vector2 screenPos = ctx.ReadValue<Vector2>(); // 마우스의 스크린 위치 (픽셀 단위)
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        // 캐릭터와 같은 높이에 위치한 지면 평면 (Y축 평면)
        Plane plane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter); // 마우스가 가리키는 지면 위 점
            Vector3 direction = hitPoint - transform.position;
            direction.y = 0; // 수직 회전 제거

            if (direction != Vector3.zero)
            {
                targetQuaternion = Quaternion.LookRotation(direction);
                transform.rotation = targetQuaternion;
            }
        }
    }
}
