using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberDisplay : MonoBehaviour
{
    public NumberFont numberFont; // 0?9のスプライトを保持するスクリプト
    public GameObject digitPrefab; // Imageコンポーネントを持つプレハブ
    public float spacing = 50f; // 各数字の間隔

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
                Image image = digitObj.GetComponent<Image>();
                if (image != null)
                {
                    image.sprite = sprite;
                }

                // RectTransformを使って配置
                RectTransform rectTransform = digitObj.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(xOffset, 0);
                xOffset += spacing; // 間隔を追加
            }
        }
    }
}
