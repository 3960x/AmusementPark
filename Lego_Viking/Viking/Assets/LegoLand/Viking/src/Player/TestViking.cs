using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestViking : MonoBehaviour
{
    // �ոӸ��� bool 
    // ����� ������ ���� �ݴ��� ����

    public float angle = 0;
    private float lerpTimer = 0;
    public float speed = 2f;
    public float SpeedValue = 0;
    float gravity = 0.0f;

    private void FixedUpdate()
    {
        lerpTimer += Time.deltaTime * speed/20;
        transform.rotation = PendulumRotation();

        if (speed > 0.0f) speed -= Time.deltaTime * gravity;
        else speed = 0;

        if (angle > 0.0f) angle -= Time.deltaTime;
        else angle = 0;

        if (SpeedValue != 0)
        {
            speed += SpeedValue;
            SpeedValue = 0;
        }

        //
        /*
         
         */

        //
    }

  
   

    Quaternion PendulumRotation()
    {
        return Quaternion.Lerp(Quaternion.Euler(Vector3.forward * angle), Quaternion.Euler(Vector3.back * angle), ((Mathf.Sin(lerpTimer) + 1.0f) * 0.5f));
    }

}
