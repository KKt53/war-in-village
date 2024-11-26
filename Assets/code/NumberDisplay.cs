using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberDisplay : MonoBehaviour
{
    public NumberFont numberFont; // 0?9�̃X�v���C�g��ێ�����X�N���v�g
    public GameObject digitPrefab; // Image�R���|�[�l���g�����v���n�u
    public float spacing = 50f; // �e�����̊Ԋu

    public void DisplayNumber(int number)
    {
        // �����̎q�I�u�W�F�N�g���N���A
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // �����𕶎���ɕϊ�
        string numberStr = number.ToString();
        float xOffset = 0;

        foreach (char digit in numberStr)
        {
            Sprite sprite = numberFont.GetSprite(digit);
            if (sprite != null)
            {
                // �v���n�u�𐶐����ăX�v���C�g��ݒ�
                GameObject digitObj = Instantiate(digitPrefab, transform);
                Image image = digitObj.GetComponent<Image>();
                if (image != null)
                {
                    image.sprite = sprite;
                }

                // RectTransform���g���Ĕz�u
                RectTransform rectTransform = digitObj.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(xOffset, 0);
                xOffset += spacing; // �Ԋu��ǉ�
            }
        }
    }
}
