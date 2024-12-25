using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Special_Guard : MonoBehaviour
{
    public bool skill_flag = false;

    public Button targetButton; // 対象のボタン
    public float cooldownTime = 20.0f; // クールダウン時間（秒）

    private float timeofeffect = 3.0f;
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

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnButtonClick();
        }
    }

    void OnButtonClick()
    {
        if (!isOnCooldown)
        {
            Debug.Log("guard");
            skill_flag = true;
            StartCooldown();
            StartCoroutine(effect_time());

            sq_instance = Instantiate(sq, canvas.transform);
            RectTransform rectTransform = sq_instance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(-200, -250);
        }
    }
    IEnumerator effect_time()
    {
        yield return new WaitForSeconds(timeofeffect);
        Debug.Log("guard end");
        skill_flag = false;
    }

    void StartCooldown()
    {
        isOnCooldown = true;
        timer = cooldownTime;
        if (targetButton != null)
        {
            targetButton.interactable = false; // ボタンを押せなくする
        }
    }

    void ResetCooldown()
    {
        isOnCooldown = false;
        timer = 0f;
        if (targetButton != null)
        {
            targetButton.interactable = true; // ボタンを再び押せるようにする
        }
    }
}
