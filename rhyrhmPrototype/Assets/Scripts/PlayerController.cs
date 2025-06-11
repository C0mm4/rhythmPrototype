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

        // ĳ���Ϳ� ���� ������ ��� (XZ ��� ��)
        Plane plane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 direction = hitPoint - transform.position;

            cursorDir = Mathf.Atan2(direction.z, direction.x); // z���� �������� ������ ��

            direction.y = 0f; // ���� ���� ����

            if (direction != Vector3.zero)
            {
                // ���콺 ������ �ٶ󺸴� ȸ��
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                // X������ 90�� ȸ���� ������Ʈ�� �°� ����
                Quaternion correction = Quaternion.Euler(90f, 0f, 0f); // x������ 90�� ȸ��
                transform.rotation = lookRotation * correction;
            }


        }
    }
}
