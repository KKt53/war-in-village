using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : Attack_Object
{
    private Transform startPoint;// 開始地点
    private Transform endPoint;// 終了地点
    public float speed = 10.0f;    // 移動速度

    public float duration = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = this.transform;
    }

    public void Initialize(int c_attack_point, List<string> c_features_point, Transform c_endPoint)
    {
        attack_point = c_attack_point;
        features_point = c_features_point;
        endPoint = c_endPoint;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPoint.position, speed * Time.deltaTime);

        if (transform.position.y <= 0)
        {
            Destroy(this.gameObject);
        }

        //t += Time.deltaTime * speed;

        //transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);


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
