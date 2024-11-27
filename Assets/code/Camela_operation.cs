using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camela_operation : MonoBehaviour
{
    Transform tf; //Main Camera��Transform
    Camera cam; //Main Camera��Camera

    void Start()
    {
        tf = this.gameObject.GetComponent<Transform>(); //Main Camera��Transform���擾����B
        cam = this.gameObject.GetComponent<Camera>(); //Main Camera��Camera���擾����B
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.I)) //I�L�[��������Ă����
        {
            if (tf.position.x >= -5)
            {
                tf.position = tf.position + new Vector3(-0.05f, 0.0f, 0.0f); //�J���������ֈړ��B
            }
        }
        else if (Input.GetKey(KeyCode.O)) //O�L�[��������Ă����
        {
            if (tf.position.x <= 5)
            {
                tf.position = tf.position + new Vector3(0.05f, 0.0f, 0.0f); //�J�������E�ֈړ��B
            }
        }
        if (Input.GetKey(KeyCode.K)) //��L�[��������Ă����
        {
            if (cam.orthographicSize <= 14)
            {
                cam.orthographicSize = cam.orthographicSize + 0.01f; //�Y�[���A�E�g�B
            }
        }
        else if (Input.GetKey(KeyCode.L)) //���L�[��������Ă����
        {
            if (cam.orthographicSize >= 2)
            {
                cam.orthographicSize = cam.orthographicSize - 0.01f; //�Y�[���C���B
            }
        }
    }
}
