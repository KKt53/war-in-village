using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camela_operation : MonoBehaviour
{
    Transform tf; //Main CameraのTransform
    Camera cam; //Main CameraのCamera

    public int max = 8;
    public int min = 3;
    public float zoomSpeed = 0.1f;

    public Transform targetObject; // スケールを変更するオブジェクト

    private float initialCameraSize;
    private Vector3 initialObjectScale;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float minSwipeDistance = 50f; // スワイプと判定する最小距離
    private float swipeSensitivity = 0.04f; // スワイプ感度（移動量の調整）

    void Start()
    {
        if (Select_Push_1.push_flg == true)
        {
            this.transform.position = Select_Push_1.Camera_position;
        }

        tf = this.gameObject.GetComponent<Transform>(); //Main CameraのTransformを取得する。
        cam = this.gameObject.GetComponent<Camera>(); //Main CameraのCameraを取得する。

        cam.orthographicSize = 4;

        initialCameraSize = cam.orthographicSize;
        initialObjectScale = targetObject.localScale;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.I)) //Iキーが押されていれば
        {
            if (tf.position.x >= -5)
            {
                tf.position = tf.position + new Vector3(-0.04f, 0.0f, 0.0f); //カメラを左へ移動。
            }
        }
        else if (Input.GetKey(KeyCode.O)) //Oキーが押されていれば
        {
            if (tf.position.x <= 5)
            {
                tf.position = tf.position + new Vector3(0.04f, 0.0f, 0.0f); //カメラを右へ移動。
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

        // タッチ操作の場合
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    // 継続的にスワイプ移動を行う場合
                    Vector2 delta = touch.deltaPosition;
                    MoveCamera(delta.x);
                    break;

                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    DetectSwipe();
                    break;
            }
        }

        // マウス操作の場合（PCでのデバッグ用）
        if (Input.GetMouseButtonDown(0)) // マウス左クリックをタッチ開始に対応
        {
            startTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0)) // マウス移動中に対応
        {
            Vector2 currentPosition = Input.mousePosition;
            Vector2 delta = currentPosition - startTouchPosition;
            MoveCamera(delta.x);
            startTouchPosition = currentPosition; // 移動後の位置を次の起点に更新
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
        // スワイプ量に基づいてカメラを移動
        Vector3 cameraPosition = cam.transform.position;
        cameraPosition.x -= swipeDeltaX * swipeSensitivity; // スワイプ方向に応じてカメラを移動

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, -5, 5);

        cam.transform.position = cameraPosition;
    }

    void DetectSwipe()
    {
        Vector2 swipeDelta = endTouchPosition - startTouchPosition;

        if (swipeDelta.magnitude < minSwipeDistance)
            return;

        // スワイプ方向の判定（オプション）
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
