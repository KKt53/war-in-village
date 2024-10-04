using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_Spawn : MonoBehaviour
{
    public GameObject canvas;
    public GameObject text_r;
    public GameObject text_l;
    public GameObject sq_r;
    public GameObject sq_l;

    GameObject sq_instance_r;
    GameObject sq_instance_l;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            sq_instance_r = Instantiate(sq_r, canvas.transform);
            RectTransform rectTransform = sq_instance_r.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(360, 50);

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sq_instance_l = Instantiate(sq_l, canvas.transform);
            RectTransform rectTransform = sq_instance_l.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(-360, 50);
        }
    }
}
