using System.Collections.Generic;
using UnityEngine;

public class NumberDisplay : MonoBehaviour
{
    public NumberFont numberFont; // 上記のNumberFontスクリプトをアタッチ
    public GameObject digitPrefab; // 1桁ごとに表示するためのプレハブ（SpriteRenderer付き）

    public void DisplayNumber(int number)
    {
        // 既存の子オブジェクトをクリア
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 数字を文字列に変換
        string numberStr = number.ToString();
        float xOffset = 0;

        foreach (char digit in numberStr)
        {
            Sprite sprite = numberFont.GetSprite(digit);
            if (sprite != null)
            {
                // プレハブを生成してスプライトを設定
                GameObject digitObj = Instantiate(digitPrefab, transform);
                digitObj.GetComponent<SpriteRenderer>().sprite = sprite;

                // 配置
                digitObj.transform.localPosition = new Vector3(xOffset, 0, 0);
                xOffset += 1.0f; // 桁の間隔を調整
            }
        }
    }
}
