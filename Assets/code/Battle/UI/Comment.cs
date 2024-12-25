using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Comment : MonoBehaviour
{
    private RectTransform rectTransform;
    private float speed;
    private int r_color;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        speed = UnityEngine.Random.Range(70f, 130f);

        r_color = UnityEngine.Random.Range(1, 4);

        TextMeshProUGUI this_color = this.GetComponent<TextMeshProUGUI>();

        if (r_color == 2)
        {
            this_color.color = Color.red;
        }
        else if(r_color == 3)
        {
            this_color.color = Color.yellow;
        }

        
    }

    public void Initialize(string s)
    { 

    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition += Vector2.left * speed * Time.deltaTime;

        if (IsOutOfScreen())
        {
            Destroy(gameObject); // テキストオブジェクトを削除
        }
    }

    private bool IsOutOfScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(rectTransform.position);
        //return rectTransform.anchoredPosition.x < -200;
        return screenPoint.x < Screen.width / 2 * -1;
    }
}
