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
    [SerializeField]
    private float cursorDir;

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
            code.OnInput(cursorDir);
        }
    }

    public void OnMouseMove(InputAction.CallbackContext ctx)
    {
        Vector2 screenPos = ctx.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        // 캐릭터와 같은 높이의 평면 (XZ 평면 위)
        Plane plane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 direction = hitPoint - transform.position;

            cursorDir = Mathf.Atan2(direction.z, direction.x); // z축을 위쪽으로 간주할 때

            direction.y = 0f; // 수직 방향 제거

            if (direction != Vector3.zero)
            {
                // 마우스 방향을 바라보는 회전
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                // X축으로 90도 회전된 오브젝트에 맞게 조정
                Quaternion correction = Quaternion.Euler(90f, 0f, 0f); // x축으로 90도 회전
                transform.rotation = lookRotation * correction;
            }


        }
    }
}
