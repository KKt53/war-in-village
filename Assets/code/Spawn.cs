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
    public Button speed_Button;

    private int speed_switch = 1;

    const int line_max = 2;
    const float Interval = 3.0f;

    public SpawnUnitTable unitTable;
    private int unitIndex = 0;
    private int random_value = 0;

    private bool isSpawning = false;  // Track if the coroutine is running

    // Start is called before the first frame update
    void Start()
    {
        max_time = 10f;
        timeBar.fillAmount = 1f;
        time_switch = false;

        unitIndex = 0;

        menu_Button.onClick.AddListener(OnButtonClick_menu);
        cancel_Button.onClick.AddListener(OnButtonClick_cancel);
        speed_Button.onClick.AddListener(OnButtonClick_speed);

        isSpawning = false;
    }

    void OnButtonClick_speed()
    {
        speed_switch++;
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
        Operation();

        if (!isSpawning)
        {
            StartCoroutine(Unit_spawn());
        }
    }

    private void Operation()
    {
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

        if (speed_switch == 1)
        {
            Time.timeScale = 1.0f;
        }
        else if (speed_switch == 2)
        {
            Time.timeScale = 1.5f;
        }
        else if (speed_switch == 3)
        {
            Time.timeScale = 2.0f;
        }
        else if (speed_switch >= 4)
        {
            speed_switch = 1;
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
    }

    IEnumerator Unit_spawn()
    {
        isSpawning = true;

        int table_max = unitTable.spawnquence.Count;

        GameObject characterInstance;
        Unit movementScript;
        List<string> features_point;
        List<string> status;

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        unitIndex = UnityEngine.Random.Range(0, table_max);
        string spawnunit = unitTable.spawnquence[unitIndex];

        switch (spawnunit)
        {
            case "Unit_1":

                characterInstance = Instantiate(characterPrefab_first, new Vector3(-10, line, 0), Quaternion.identity);

                movementScript = characterInstance.GetComponent<Unit>();

                features_point = new List<string> { "大型BOSSに強い", "中型" };

                status = new List<string> { "攻撃力アップ", "移動速度アップダウン" };

                movementScript.Initialize(3, 1, features_point, 3f, 0.5f, 5.0f, 1, 4, status); //ヒットポイント,攻撃力,ダメージ増減倍率,素早さ,反応速度,攻撃頻度,大きさ,攻撃範囲,かかりやすい状態

                yield return new WaitForSeconds(Interval);

                break;
            case "Unit_2":

                characterInstance = Instantiate(characterPrefab_second, new Vector3(-10, line, 0), Quaternion.identity);

                movementScript = characterInstance.GetComponent<Unit>();

                features_point = new List<string> { "大型BOSSに強い", "中型" };

                status = new List<string> { "攻撃力アップ", "移動速度アップダウン" };

                movementScript.Initialize(3, 1, features_point, 2f, 0.8f, 1.0f, 1, 5, status);

                yield return new WaitForSeconds(Interval);

                break;

            case "Unit_3":

                characterInstance = Instantiate(characterPrefab_third, new Vector3(-10, line, 0), Quaternion.identity);

                movementScript = characterInstance.GetComponent<Unit>();

                features_point = new List<string> { "大型BOSSに強い", "中型" };

                status = new List<string> { "攻撃力アップ", "移動速度アップダウン" };

                movementScript.Initialize(1, 1, features_point, 4f, 0.1f, 10.0f, 1, 3, status);

                yield return new WaitForSeconds(Interval);

                break;
        }

        isSpawning = false;
    }
}
