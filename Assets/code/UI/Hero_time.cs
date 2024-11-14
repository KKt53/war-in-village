using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_time : MonoBehaviour
{
    private float duration = 180f; // 右端に到達するまでの時間（秒）
    private RectTransform rectTransform;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float elapsedTime = 0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // UIの左端の開始位置
        startPosition = new Vector2(0 - ((RectTransform)rectTransform.parent).rect.width / 2 + 50, rectTransform.anchoredPosition.y);

        // UIの右端の目的位置（親のサイズに基づく）
        endPosition = new Vector2(((RectTransform)rectTransform.parent).rect.width / 2 - 50, rectTransform.anchoredPosition.y);

        // 初期位置に設定
        rectTransform.anchoredPosition = startPosition;
    }

    void Update()
    {
        // 経過時間をカウント
        elapsedTime += Time.deltaTime;

        // 補間の割合を計算
        float t = Mathf.Clamp01(elapsedTime / duration);

        // 線形補間でUIの位置を更新
        rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

        // 指定した時間が経過したら動きを止める
        if (elapsedTime >= duration)
        {
            enabled = false;
        }
    }
}
