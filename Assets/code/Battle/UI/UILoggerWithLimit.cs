using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILoggerWithLimit : MonoBehaviour
{
    [SerializeField] private GameObject logPrefab; // �쐬�������O���b�Z�[�W�̃v���n�u
    [SerializeField] private Transform contentParent; // Content�I�u�W�F�N�g���w��
    [SerializeField] private int maxLogCount = 10; // �ő�\�����b�Z�[�W��

    private Queue<GameObject> logMessages = new Queue<GameObject>();

    // ���O��ǉ����郁�\�b�h
    public void AddLog(string message)
    {
        // �V�������O�̃C���X�^���X���쐬
        GameObject newLog = Instantiate(logPrefab, contentParent);

        // �q�I�u�W�F�N�g���� TMP_Text �R���|�[�l���g���擾
        TMP_Text logText = newLog.GetComponentInChildren<TMP_Text>();
        logText.text = message;

        // ���O�L���[�ɒǉ����A�\������郍�O���ő吔�𒴂����ꍇ�͌Â����O���폜
        logMessages.Enqueue(newLog);

        // �ő吔�𒴂����ꍇ�A��ԉ��̌Â����O���폜
        if (logMessages.Count > maxLogCount)
        {
            GameObject oldLog = logMessages.Dequeue();
            Destroy(oldLog); // �Â����O���폜
        }

        // �R���e���c�̃X�N���[���ʒu���ŏ㕔�ɕۂ�
        Canvas.ForceUpdateCanvases(); // �L�����o�X�̍X�V
        //contentParent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // �R���e���c�̈ʒu�����Z�b�g
    }

    // ���O���N���A���郁�\�b�h
    public void ClearLogs()
    {
        while (logMessages.Count > 0)
        {
            Destroy(logMessages.Dequeue());
        }
    }
}
