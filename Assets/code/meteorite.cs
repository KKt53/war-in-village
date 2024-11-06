using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteorite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= 0)
        {
            transform.Translate(Vector2.left * 2 * Time.deltaTime);
            transform.Translate(Vector2.down * 2.2f * Time.deltaTime);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }
}
