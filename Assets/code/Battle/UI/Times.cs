using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Times : MonoBehaviour
{
    public Button menu_Button;
    public Button cancel_Button;
    public Button speed_Button;
    public GameObject s_button;

    private bool time_switch = false;

    private float duration = 180f; // 右端に到達するまでの時間（秒）
    private RectTransform rectTransform;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float elapsedTime = 0f;

    public GameObject Panel;
    public GameObject Over;

    public Sprite speed_off;
    public Sprite speed_on;

    private int speed_switch = 1;

    Image i_s_button;

    GameObject boss;//ボス用変数

    Boss boss_i;

    void Start()
    {
        if (Select_Push_1.push_flg == true)
        {
            duration = Select_Push_1.stage_time;
        }

        time_switch = false;

        rectTransform = GetComponent<RectTransform>();

        // UIの左端の開始位置
        startPosition = new Vector2(0 - ((RectTransform)rectTransform.parent).rect.width / 2 + 50, rectTransform.anchoredPosition.y);

        // UIの右端の目的位置（親のサイズに基づく）
        endPosition = new Vector2(((RectTransform)rectTransform.parent).rect.width / 2 - 50, rectTransform.anchoredPosition.y);

        // 初期位置に設定
        rectTransform.anchoredPosition = startPosition;

        menu_Button.onClick.AddListener(OnButtonClick_menu);
        cancel_Button.onClick.AddListener(OnButtonClick_cancel);
        speed_Button.onClick.AddListener(OnButtonClick_speed);

        i_s_button = s_button.GetComponent<Image>();

        boss = GameObject.Find("Boss");

        boss_i = boss.GetComponent<Boss>();
    }

    void Update()
    {
        Operation();

        gamgeover();

        if (boss_i.hp <= 0)
        {
            gamgeclear();
        }
    }

    private void Operation()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (time_switch == false)
            {
                time_switch = true;
            }
            else
            {
                time_switch = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnButtonClick_speed();
        }

        if (time_switch == true)
        {
            Panel.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else
        {
            Panel.SetActive(false);
            if (speed_switch == 1)
            {
                Time.timeScale = 1.0f;
                i_s_button.sprite = speed_off;
            }
            else if (speed_switch == 2)
            {
                Time.timeScale = 2.0f;
                i_s_button.sprite = speed_on;
            }
            else if (speed_switch >= 3)
            {
                speed_switch = 1;
            }
        }
    }

    void gamgeover()
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
            Time.timeScale = 0.0f;

            Over.SetActive(true);
            GameObject child = Over.transform.Find("オーバー文字").gameObject;
            TMP_Text c_Text = child.GetComponentInChildren<TMP_Text>();

            c_Text.text = "GAME OVER";

            enabled = false;
        }
    }

    void gamgeclear()
    {
        Time.timeScale = 0.0f;

        Over.SetActive(true);
        GameObject child = Over.transform.Find("オーバー文字").gameObject;
        TMP_Text c_Text = child.GetComponentInChildren<TMP_Text>();

        c_Text.text = "GAME CLEAR";
    }

    void OnButtonClick_menu()
    {
        if (Over.activeSelf == false)
        {
            time_switch = true;
        }

        
    }

    void OnButtonClick_cancel()
    {
        if (Over.activeSelf == false)
        {
            time_switch = false;
        }
    }

    void OnButtonClick_speed()
    {
        if (Over.activeSelf == false)
        {
            speed_switch++;
        }
    }
}
