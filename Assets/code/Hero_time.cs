using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_time : MonoBehaviour
{
    private float duration = 180f; // �E�[�ɓ��B����܂ł̎��ԁi�b�j
    private RectTransform rectTransform;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float elapsedTime = 0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // UI�̍��[�̊J�n�ʒu
        startPosition = new Vector2(0 - ((RectTransform)rectTransform.parent).rect.width / 2 + 50, rectTransform.anchoredPosition.y);

        // UI�̉E�[�̖ړI�ʒu�i�e�̃T�C�Y�Ɋ�Â��j
        endPosition = new Vector2(((RectTransform)rectTransform.parent).rect.width / 2 - 50, rectTransform.anchoredPosition.y);

        // �����ʒu�ɐݒ�
        rectTransform.anchoredPosition = startPosition;
    }

    void Update()
    {
        // �o�ߎ��Ԃ��J�E���g
        elapsedTime += Time.deltaTime;

        // ��Ԃ̊������v�Z
        float t = Mathf.Clamp01(elapsedTime / duration);

        // ���`��Ԃ�UI�̈ʒu���X�V
        rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

        // �w�肵�����Ԃ��o�߂����瓮�����~�߂�
        if (elapsedTime >= duration)
        {
            enabled = false;
        }
    }
}
