using System.Collections.Generic;
using UnityEngine;

public class NumberDisplay : MonoBehaviour
{
    public NumberFont numberFont; // ��L��NumberFont�X�N���v�g���A�^�b�`
    public GameObject digitPrefab; // 1�����Ƃɕ\�����邽�߂̃v���n�u�iSpriteRenderer�t���j

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
                digitObj.GetComponent<SpriteRenderer>().sprite = sprite;

                // �z�u
                digitObj.transform.localPosition = new Vector3(xOffset, 0, 0);
                xOffset += 1.0f; // ���̊Ԋu�𒲐�
            }
        }
    }
}
