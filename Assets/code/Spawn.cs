using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Spawn : MonoBehaviour
{
    //名前は後で変更する
    public GameObject characterPrefab_first;
    public GameObject characterPrefab_second;
    public GameObject characterPrefab_third;

    public GameObject Panel;

    private bool time_switch = false;

    public float max_time = 160f;

    public Image timeBar;   // タイムゲージのImage

    public Button menu_Button;
    public Button cancel_Button;

    // Start is called before the first frame update
    void Start()
    {
        max_time = 10f;
        timeBar.fillAmount = 1f;
        time_switch = false;

        menu_Button.onClick.AddListener(OnButtonClick_menu);
        cancel_Button.onClick.AddListener(OnButtonClick_cancel);
    }

    void OnButtonClick_menu()
    {
        time_switch = true;
    }

    void OnButtonClick_cancel()
    {
        time_switch = false;
    }

    void Update()
    {
        GameObject characterInstance;
        Unit movementScript;
        List<string> features_point;
        List<string> status;

        double limit_time = Math.Floor(max_time - Time.time) / max_time;

        timeBar.fillAmount = (float)limit_time;

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

        if (time_switch == true)
        {
            Panel.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else
        {
            Panel.SetActive(false);
            Time.timeScale = 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            characterInstance = Instantiate(characterPrefab_first, new Vector3(-10, 0, 0), Quaternion.identity);

            movementScript = characterInstance.GetComponent<Unit>();

            features_point = new List<string> { "大型BOSSに強い", "中型" };

            status = new List<string> { "攻撃力アップ", "移動速度アップダウン" };

            movementScript.Initialize(2, 1, features_point, 3f, 0.5f, 5.0f, 1, 4, status); //ヒットポイント,攻撃力,ダメージ増減倍率,素早さ,反応速度,攻撃頻度,大きさ,攻撃範囲,かかりやすい状態
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            characterInstance = Instantiate(characterPrefab_second, new Vector3(-10, 0, 0), Quaternion.identity);

            movementScript = characterInstance.GetComponent<Unit>();

            features_point = new List<string> { "大型BOSSに強い", "中型" };

            status = new List<string> { "攻撃力アップ", "移動速度アップダウン" };

            movementScript.Initialize(3, 1, features_point, 2f, 0.8f, 1.0f, 1, 5, status);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            characterInstance = Instantiate(characterPrefab_third, new Vector3(-10, 0, 0), Quaternion.identity);

            movementScript = characterInstance.GetComponent<Unit>();

            features_point = new List<string> { "大型BOSSに強い", "中型" };

            status = new List<string> { "攻撃力アップ", "移動速度アップダウン" };

            movementScript.Initialize(1, 1, features_point, 4f, 0.1f, 10.0f, 1, 3, status);
        }
    }
}
