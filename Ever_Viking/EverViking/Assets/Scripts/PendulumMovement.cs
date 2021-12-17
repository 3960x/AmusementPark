using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumMovement : MonoBehaviour
{
    // >>>> ������ ���� ������Ʈ >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    [Header("������ ���")]
    [Header("=============================================")]
    public GameObject MovingObj;

    [Header("����ŷ�� �Ӹ��� �������� �� ����")]
    public GameObject VikingPolarLeft;
    public GameObject VikingPolarRight;

    [Header("���� ��ư ������ ���� ��ü ������Ʈ")]
    public GameObject BtnSpawnerObj;

    [Header("��ġ�� ��ư�� ������ ��ġ")]
    public GameObject BtnSpawnPoint1;
    public GameObject BtnSpawnPoint2;




    // >>>> �ܺ� ���� ������ ���� >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    [Header("����ŷ ȸ�� �ӵ�")]
    [Header("=============================================")]
    public float Speed = 1f;

    [Header("�ʱ� ��� ȸ����")]
    [Range(20f, 285)]
    public float FIRST_POLAR_ROTATION = 30f;




    // >>>> ���� ���� ���� >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    private float timer;                                // ȸ�� Ÿ�̸�
    private float leftRotX;                             // �±� ȸ����
    private float rightRotX;                            // ��� ȸ����
    private float prevRotX;                             // ���� ȸ����
    private float curRotX;                              // ���� ȸ����
    private float currentPolarRot;                      // ���� ��� ȸ����
    private bool bLeftPolarRotChanged;                  // �±ش� ȸ������ ��ȭ�ߴ��� ����
    private bool bRightPolarRotChanged;                 // ��ش� ȸ������ ��ȭ�ߴ��� ����




    // >>>> ������Ƽ >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    public float CurrentPolarRot { get { return currentPolarRot; } }




    // >>>> �Լ� >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    // Start is called before the first frame update
    void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        // ���� �ʱ�ȭ
        timer = 0f;
        currentPolarRot = FIRST_POLAR_ROTATION;
        leftRotX = -FIRST_POLAR_ROTATION;
        rightRotX = FIRST_POLAR_ROTATION;
        prevRotX = 0f;
        curRotX = 0f;
        bLeftPolarRotChanged = false;
        bRightPolarRotChanged = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        timer += Time.fixedDeltaTime;

        float t = (Mathf.Sin(timer * Speed + Mathf.PI / 2f) + 1f) / 2f;
        CheckPolarChanged(t);
        prevRotX = curRotX;
        curRotX = Mathf.Lerp(leftRotX, rightRotX, t);
        MovingObj.transform.localRotation = Quaternion.Euler(curRotX, 0f, 0f);

        //Debug.Log("t: " + t);
    }

    private void CheckPolarChanged(float t)
    {
        // �� ���� ��ȭ���� �ʾҴٸ� ����
        if(!bLeftPolarRotChanged && !bRightPolarRotChanged)   return;

        // ����ŷ�� �� �ش��� �����Ѵ�..
        if(t < 0.01f && bRightPolarRotChanged)
        {
            rightRotX = currentPolarRot;
            bRightPolarRotChanged = false;
            Debug.Log("Right Rot X: " + rightRotX);
        }
        else if(t > 0.99f && bLeftPolarRotChanged)
        {
            leftRotX = -currentPolarRot;
            bLeftPolarRotChanged = false;
            Debug.Log("Left Rot X: " + leftRotX);
        }
    }

    public void SetPolarRotation(float rot)
    {
        currentPolarRot = rot;
        bLeftPolarRotChanged = true;
        bRightPolarRotChanged = true;
    }

    public Vector3 GetRandomTouchBtnSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;

        // ��ư�� ��ȿ�� ���� ���� ����Ʈ�� ��� ���� ��ư ������ ����ŷ�� ������ ��ġ�� ȸ����Ų��.
        float t = UnityEngine.Random.Range(0f, 1f);
        float tempLeftRotX = -CurrentPolarRot;
        float tempRightRotX = CurrentPolarRot;
        float randomRotX = Mathf.Lerp(tempLeftRotX, tempRightRotX, t);
        BtnSpawnerObj.transform.localRotation = Quaternion.Euler(randomRotX, 0f, 0f);

        // ���Ƿ� ȸ����Ų ��ġ���� ���� ����Ʈ�� ��ǥ���� ���´�.
        int randomSelector = UnityEngine.Random.Range(0, 2);
        switch(randomSelector)
        {
            case 0:
                spawnPoint = BtnSpawnPoint1.transform.position;
                break;
            case 1:
                spawnPoint = BtnSpawnPoint2.transform.position;
                break;
        }

        return spawnPoint;
    }

    public Vector3 GetHeadPosition()
    {
        return (curRotX - prevRotX > 0) ? 
            VikingPolarLeft.transform.position : VikingPolarRight.transform.position;
    }

    public Vector3 GetTailPosition()
    {
        return (curRotX - prevRotX > 0) ? 
            VikingPolarRight.transform.position : VikingPolarLeft.transform.position;
    }
}
