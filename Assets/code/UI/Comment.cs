using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comment : MonoBehaviour
{
    private RectTransform rectTransform;
    private float speed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition += Vector2.left * speed * Time.deltaTime;

        if (IsOutOfScreen())
        {
            Debug.Log(rectTransform.anchoredPosition);
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
