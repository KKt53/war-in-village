using UnityEngine;

public class ExampleLoggerCaller : MonoBehaviour
{
    private UILoggerWithLimit uiLogger;

    private void Start()
    {
        // UILoggerWithLimit�R���|�[�l���g�̎Q�Ƃ��擾
        uiLogger = FindObjectOfType<UILoggerWithLimit>();
    }

    private void Update()
    {
        // ��Ƃ��āA�L�[���͂ɉ����ă��O��ǉ�
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddExampleLog();
        }
    }

    private void AddExampleLog()
    {
        if (uiLogger != null)
        {
            uiLogger.AddLog("�V�������O���b�Z�[�W: " + Time.time);
        }
        else
        {
            Debug.LogWarning("UILoggerWithLimit��������܂���ł���");
        }
    }
}
