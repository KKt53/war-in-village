using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    private Transform startPoint;// 開始地点
    private Transform endPoint;// 終了地点
    public float speed = 0.000001f;    // 移動速度
    private float t = 0; // 補間値

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
