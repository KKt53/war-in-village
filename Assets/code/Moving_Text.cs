using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Moving_Text : MonoBehaviour
{
    public float amplitude = 100.0f; // ���V�̍���
    public float frequency = 1f;   // ���V�̑���

    private Vector3 startPos;

    float Sin;
    float newY;

    float start_time;
    float upd_time;

    private Image image; // Image�R���|�[�l���g���i�[���邽�߂̕ϐ�

    // Start is called before the first frame update
    void Start()
    {
        image = this.gameObject.GetComponentInChildren<Image>();

        // �I�u�W�F�N�g�̏����ʒu���L�^
        startPos = transform.position;

        start_time = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        upd_time = Time.time - start_time;

        Sin = Mathf.Sin(upd_time * frequency);

        //SetTransparency(upd_time / 2);

        // Sin�֐����g�p����Y���̈ʒu���㉺�ɕύX
        newY = (Sin * amplitude) * 100;

        // �V�����ʒu��ݒ�iX��Z�͕ς�����Y��������ϓ�������j
        transform.position = new Vector3(startPos.x, startPos.y + newY, startPos.z);

        if (upd_time >= 2)
        {
            Destroy(this.gameObject);
            start_time = 0;
        }
    }

    public void SetTransparency(float alpha)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = Mathf.Clamp(alpha, 0f, 1f); // �A���t�@�l��0�`1�ɐ���
            image.color = color;
        }
    }
}
