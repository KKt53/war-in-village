using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILoggerWithLimit : MonoBehaviour
{
    [SerializeField] private GameObject logPrefab; // 作成したログメッセージのプレハブ
    [SerializeField] private Transform contentParent; // Contentオブジェクトを指定
    [SerializeField] private int maxLogCount = 10; // 最大表示メッセージ数

    private Queue<GameObject> logMessages = new Queue<GameObject>();

    // ログを追加するメソッド
    public void AddLog(string message)
    {
        // 新しいログのインスタンスを作成
        GameObject newLog = Instantiate(logPrefab, contentParent);

        // 子オブジェクトから TMP_Text コンポーネントを取得
        TMP_Text logText = newLog.GetComponentInChildren<TMP_Text>();
        logText.text = message;

        // ログキューに追加し、表示されるログが最大数を超えた場合は古いログを削除
        logMessages.Enqueue(newLog);

        // 最大数を超えた場合、一番下の古いログを削除
        if (logMessages.Count > maxLogCount)
        {
            GameObject oldLog = logMessages.Dequeue();
            Destroy(oldLog); // 古いログを削除
        }

        // コンテンツのスクロール位置を最上部に保つ
        Canvas.ForceUpdateCanvases(); // キャンバスの更新
        //contentParent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // コンテンツの位置をリセット
    }

    // ログをクリアするメソッド
    public void ClearLogs()
    {
        while (logMessages.Count > 0)
        {
            Destroy(logMessages.Dequeue());
        }
    }
}
