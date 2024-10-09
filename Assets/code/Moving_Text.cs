using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Moving_Text : MonoBehaviour
{
    public float amplitude = 100.0f; // 浮遊の高さ
    public float frequency = 1f;   // 浮遊の速さ

    private Vector3 startPos;

    float Sin;
    float newY;

    float start_time;
    float upd_time;

    private Image image; // Imageコンポーネントを格納するための変数

    // Start is called before the first frame update
    void Start()
    {
        image = this.gameObject.GetComponentInChildren<Image>();

        // オブジェクトの初期位置を記録
        startPos = transform.position;

        start_time = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        upd_time = Time.time - start_time;

        Sin = Mathf.Sin(upd_time * frequency);

        //SetTransparency(upd_time / 2);

        // Sin関数を使用してY軸の位置を上下に変更
        newY = (Sin * amplitude) * 100;

        // 新しい位置を設定（XとZは変えずにY軸だけを変動させる）
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
            color.a = Mathf.Clamp(alpha, 0f, 1f); // アルファ値を0～1に制限
            image.color = color;
        }
    }
}
