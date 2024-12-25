using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Special_Storm : MonoBehaviour
{
    public bool skill_flag = false;

    public Button targetButton; // �Ώۂ̃{�^��
    public float cooldownTime = 30.0f; // �N�[���_�E�����ԁi�b�j

    private float timeofeffect = 5.0f; 
    private float timer;
    private bool isOnCooldown;

    public GameObject canvas;

    public GameObject sq;

    GameObject sq_instance;

    void Start()
    {
        if (targetButton != null)
        {
            targetButton.onClick.AddListener(OnButtonClick);
        }
        ResetCooldown();
    }

    void Update()
    {
        if (isOnCooldown)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                ResetCooldown();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnButtonClick();
        }
    }

    void OnButtonClick()
    {
        if (!isOnCooldown)
        {
            Debug.Log("storm");
            skill_flag = true;
            StartCooldown();
            StartCoroutine(effect_time());

            sq_instance = Instantiate(sq, canvas.transform);
            RectTransform rectTransform = sq_instance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(-420, -250);
        }
    }

    IEnumerator effect_time()
    {
        yield return new WaitForSeconds(timeofeffect);
        Debug.Log("storm end");
        skill_flag = false;
    }

    void StartCooldown()
    {
        isOnCooldown = true;
        timer = cooldownTime;
        if (targetButton != null)
        {
            targetButton.interactable = false; // �{�^���������Ȃ�����
        }
    }

    void ResetCooldown()
    {
        isOnCooldown = false;
        timer = 0f;
        if (targetButton != null)
        {
            targetButton.interactable = true; // �{�^�����Ăщ�����悤�ɂ���
        }
    }

}
