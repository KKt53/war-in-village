using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camela_operation : MonoBehaviour
{
    Transform tf; //Main Camera��Transform
    Camera cam; //Main Camera��Camera

    public int max = 8;
    public int min = 3;
    public float zoomSpeed = 0.1f;

    public Transform targetObject; // �X�P�[����ύX����I�u�W�F�N�g

    private float initialCameraSize;
    private Vector3 initialObjectScale;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float minSwipeDistance = 50f; // �X���C�v�Ɣ��肷��ŏ�����
    private float swipeSensitivity = 0.04f; // �X���C�v���x�i�ړ��ʂ̒����j

    void Start()
    {
        if (Select_Push_1.push_flg == true)
        {
            this.transform.position = Select_Push_1.Camera_position;
        }

        tf = this.gameObject.GetComponent<Transform>(); //Main Camera��Transform���擾����B
        cam = this.gameObject.GetComponent<Camera>(); //Main Camera��Camera���擾����B

        cam.orthographicSize = 4;

        initialCameraSize = cam.orthographicSize;
        initialObjectScale = targetObject.localScale;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.I)) //I�L�[��������Ă����
        {
            if (tf.position.x >= -5)
            {
                tf.position = tf.position + new Vector3(-0.04f, 0.0f, 0.0f); //�J���������ֈړ��B
            }
        }
        else if (Input.GetKey(KeyCode.O)) //O�L�[��������Ă����
        {
            if (tf.position.x <= 5)
            {
                tf.position = tf.position + new Vector3(0.04f, 0.0f, 0.0f); //�J�������E�ֈړ��B
            }
        }

        mouse_move();
    }

    void mouse_move()
    {
        var scroll = Input.mouseScrollDelta.y * -1;

        cam.orthographicSize = cam.orthographicSize + scroll;

        if (cam.orthographicSize >= max)
        {
            cam.orthographicSize = max;
        }

        if (cam.orthographicSize <= min)
        {
            cam.orthographicSize = min;
        }

        float scaleRatio = cam.orthographicSize / initialCameraSize;
        targetObject.localScale = initialObjectScale * scaleRatio;

        // �^�b�`����̏ꍇ
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    // �p���I�ɃX���C�v�ړ����s���ꍇ
                    Vector2 delta = touch.deltaPosition;
                    MoveCamera(delta.x);
                    break;

                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    DetectSwipe();
                    break;
            }
        }

        // �}�E�X����̏ꍇ�iPC�ł̃f�o�b�O�p�j
        if (Input.GetMouseButtonDown(0)) // �}�E�X���N���b�N���^�b�`�J�n�ɑΉ�
        {
            startTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0)) // �}�E�X�ړ����ɑΉ�
        {
            Vector2 currentPosition = Input.mousePosition;
            Vector2 delta = currentPosition - startTouchPosition;
            MoveCamera(delta.x);
            startTouchPosition = currentPosition; // �ړ���̈ʒu�����̋N�_�ɍX�V
        }

        //if (tf.position.x >= -5)
        //{
        //    tf.position = new Vector3(-5.00f, 0.0f, 0.0f); ;
        //}

        //if (tf.position.x >= 5)
        //{
        //    tf.position = new Vector3(5.00f, 0.0f, 0.0f); ;
        //}
    }

    void MoveCamera(float swipeDeltaX)
    {
        // �X���C�v�ʂɊ�Â��ăJ�������ړ�
        Vector3 cameraPosition = cam.transform.position;
        cameraPosition.x -= swipeDeltaX * swipeSensitivity; // �X���C�v�����ɉ����ăJ�������ړ�

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, -5, 5);

        cam.transform.position = cameraPosition;
    }

    void DetectSwipe()
    {
        Vector2 swipeDelta = endTouchPosition - startTouchPosition;

        if (swipeDelta.magnitude < minSwipeDistance)
            return;

        // �X���C�v�����̔���i�I�v�V�����j
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x > 0)
            {
                Debug.Log("Right Swipe");
            }
            else
            {
                Debug.Log("Left Swipe");
            }
        }
    }
}
