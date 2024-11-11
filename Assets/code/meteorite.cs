using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    private Transform startPoint;// �J�n�n�_
    private Transform endPoint;// �I���n�_
    public float speed = 0.000001f;    // �ړ����x
    private float t = 0; // ��Ԓl

    // Start is called before the first frame update
    void Start()
    {
        startPoint = this.transform;
    }

    public void Initialize(Transform c_endPoint)
    {
        endPoint = c_endPoint;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * speed;

        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);

        if (transform.position.y <= 0)
        {
            Destroy(this.gameObject);
        }
        //if (transform.position.y >= 0)
        //{
        //    transform.Translate(Vector2.left * 2 * Time.deltaTime);
        //    transform.Translate(Vector2.down * 2.2f * Time.deltaTime);

        //}
        //else
        //{
        //    Debug.Log(this.transform.position.x);
        //    Destroy(this.gameObject);
        //}

    }
}
