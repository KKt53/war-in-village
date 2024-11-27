using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camela_operation : MonoBehaviour
{
    Transform tf; //Main CameraのTransform
    Camera cam; //Main CameraのCamera

    void Start()
    {
        tf = this.gameObject.GetComponent<Transform>(); //Main CameraのTransformを取得する。
        cam = this.gameObject.GetComponent<Camera>(); //Main CameraのCameraを取得する。
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.I)) //Iキーが押されていれば
        {
            if (tf.position.x >= -5)
            {
                tf.position = tf.position + new Vector3(-0.05f, 0.0f, 0.0f); //カメラを左へ移動。
            }
        }
        else if (Input.GetKey(KeyCode.O)) //Oキーが押されていれば
        {
            if (tf.position.x <= 5)
            {
                tf.position = tf.position + new Vector3(0.05f, 0.0f, 0.0f); //カメラを右へ移動。
            }
        }
        if (Input.GetKey(KeyCode.K)) //上キーが押されていれば
        {
            if (cam.orthographicSize <= 14)
            {
                cam.orthographicSize = cam.orthographicSize + 0.01f; //ズームアウト。
            }
        }
        else if (Input.GetKey(KeyCode.L)) //下キーが押されていれば
        {
            if (cam.orthographicSize >= 2)
            {
                cam.orthographicSize = cam.orthographicSize - 0.01f; //ズームイン。
            }
        }
    }
}
