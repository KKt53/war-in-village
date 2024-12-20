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

    private float duration = 180f; // �E�[�ɓ��B����܂ł̎��ԁi�b�j
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

    GameObject boss;//�{�X�p�ϐ�

    Boss boss_i;

    void Start()
    {
        if (Select_Push_1.push_flg == true)
        {
            duration = Select_Push_1.stage_time;
        }

        time_switch = false;

        rectTransform = GetComponent<RectTransform>();

        // UI�̍��[�̊J�n�ʒu
        startPosition = new Vector2(0 - ((RectTransform)rectTransform.parent).rect.width / 2 + 50, rectTransform.anchoredPosition.y);

        // UI�̉E�[�̖ړI�ʒu�i�e�̃T�C�Y�Ɋ�Â��j
        endPosition = new Vector2(((RectTransform)rectTransform.parent).rect.width / 2 - 50, rectTransform.anchoredPosition.y);

        // �����ʒu�ɐݒ�
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
        // �o�ߎ��Ԃ��J�E���g
        elapsedTime += Time.deltaTime;

        // ��Ԃ̊������v�Z
        float t = Mathf.Clamp01(elapsedTime / duration);

        // ���`��Ԃ�UI�̈ʒu���X�V
        rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

        // �w�肵�����Ԃ��o�߂����瓮�����~�߂�
        if (elapsedTime >= duration)
        {
            Time.timeScale = 0.0f;

            Over.SetActive(true);
            GameObject child = Over.transform.Find("�I�[�o�[����").gameObject;
            TMP_Text c_Text = child.GetComponentInChildren<TMP_Text>();

            c_Text.text = "GAME OVER";

            enabled = false;
        }
    }

    void gamgeclear()
    {
        Time.timeScale = 0.0f;

        Over.SetActive(true);
        GameObject child = Over.transform.Find("�I�[�o�[����").gameObject;
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
