using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    public GameObject[] Missiles;
    public float Range = 5.0f;
    public float Interval;
    public int Lifetime = 5;

    IEnumerator Start() //�����ð����� ����
    {
        while (true)
        {
            transform.position = new Vector3( Random.Range(-Range, Range)
                                           , transform.position.y,
                                            transform.position.z);
            GameObject obj = Instantiate(
                                                        Missiles[Random.Range(0,Missiles.Length)], 
                                                        transform.position,
                                                        Quaternion.Euler(90.0f,0,0)); //object ����
            
            Destroy(obj, Lifetime);
            yield return new WaitForSeconds(Interval); //interval �Ŀ� �ٽ� ȣ��
        }
    }


}
