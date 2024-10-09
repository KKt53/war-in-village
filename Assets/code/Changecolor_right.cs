using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Changecolor_right : MonoBehaviour
{
    private Image image; // Imageコンポーネントを格納するための変数
    private Color normalColor = Color.white;  // 通常時の色
    private Color pressedColor = Color.gray; // キーを押したときの色

    // Start is called before the first frame update
    void Start()
    {
        image = this.gameObject.GetComponentInChildren<Image>();

        image.color = normalColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            image.color = pressedColor;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            image.color = normalColor;
        }
    }
}
