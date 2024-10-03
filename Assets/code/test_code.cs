using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_code : MonoBehaviour
{
    public float knockbackStrength = 5f; // �m�b�N�o�b�N�̋���
    public float knockbackDuration = 0.5f; // �m�b�N�o�b�N�̎�������

    private Vector3 knockbackDirection;
    private float knockbackTimer = 0f; // �m�b�N�o�b�N�̎��Ԃ�ǐ�
    private bool isKnockbackActive = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            isKnockbackActive = true;
        }

        // �m�b�N�o�b�N���A�N�e�B�u���ǂ������m�F
        if (isKnockbackActive)
        {
            // �m�b�N�o�b�N���܂��p�����ł���Έړ�������
            if (knockbackTimer > 0)
            {
                // ���Ԃɉ����Č������铮����������
                float knockbackSpeed = knockbackStrength * (knockbackTimer / knockbackDuration);
                transform.Translate(knockbackDirection * knockbackSpeed * Time.deltaTime);

                // �m�b�N�o�b�N���Ԃ�����
                knockbackTimer -= Time.deltaTime;
            }
            else
            {
                // �m�b�N�o�b�N�I��
                isKnockbackActive = false;
            }
        }
    }

    // �m�b�N�o�b�N���J�n����֐�
    public void ApplyKnockback(Vector3 direction)
    {
        knockbackDirection = direction.normalized; // �m�b�N�o�b�N�̕����𐳋K��
        knockbackTimer = knockbackDuration; // �m�b�N�o�b�N�^�C�}�[�����Z�b�g
        isKnockbackActive = true; // �m�b�N�o�b�N��L����
    }
}
