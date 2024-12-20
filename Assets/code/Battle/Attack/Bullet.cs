using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : Attack_Object
{
    GameObject left_edge;
    GameObject right_edge;

    private float speed;//ëfëÅÇ≥

    // Start is called before the first frame update
    void Start()
    {
        speed = 10;
        left_edge = GameObject.Find("ç∂í[");
        right_edge = GameObject.Find("âEí[");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;

        position.x = Mathf.Clamp(position.x, Mathf.Min(left_edge.transform.position.x, right_edge.transform.position.x), Mathf.Max(left_edge.transform.position.x, right_edge.transform.position.x));

        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (position.x <= -10) 
        {
            Destroy(this.gameObject);
        }
    }
}
