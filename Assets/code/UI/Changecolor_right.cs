using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Changecolor_right : MonoBehaviour
{
    private Image image; // Image�R���|�[�l���g���i�[���邽�߂̕ϐ�
    private Color normalColor = Color.white;  // �ʏ펞�̐F
    private Color pressedColor = Color.gray; // �L�[���������Ƃ��̐F

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
