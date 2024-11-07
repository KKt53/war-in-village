using UnityEngine;

public class ExampleLoggerCaller : MonoBehaviour
{
    private UILoggerWithLimit uiLogger;

    private void Start()
    {
        // UILoggerWithLimitコンポーネントの参照を取得
        uiLogger = FindObjectOfType<UILoggerWithLimit>();
    }

    private void Update()
    {
        // 例として、キー入力に応じてログを追加
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddExampleLog();
        }
    }

    private void AddExampleLog()
    {
        if (uiLogger != null)
        {
            uiLogger.AddLog("新しいログメッセージ: " + Time.time);
        }
        else
        {
            Debug.LogWarning("UILoggerWithLimitが見つかりませんでした");
        }
    }
}
