using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

public class Spawn : MonoBehaviour
{
    //名前は後で変更する
    public GameObject characterPrefab_first;
    public GameObject characterPrefab_second;
    public GameObject characterPrefab_third;

    public GameObject enemyPrefab;

    public GameObject Panel;

    private bool time_switch = false;

    public float max_time = 160f;

    public Image timeBar;   // タイムゲージのImage

    public Button menu_Button;
    public Button cancel_Button;
    public Button speed_Button;

    private int speed_switch = 1;

    const int line_max = 2;
    const float Interval = 1.0f;
    const float Interval_e = 1.0f;

    public SpawnUnitTable unitTable;
    public SpawnEnemyTable enemyTable;
    private int unitIndex = 0;
    private int random_value = 0;

    private bool isSpawning_villager = false;  // Track if the coroutine is running
    private bool isSpawning_enemy = false;

    private string spawnunit;
    private string spawnunit_after;

    // Start is called before the first frame update
    void Start()
    {
        timeBar.fillAmount = 1f;
        time_switch = false;

        unitIndex = 0;

        menu_Button.onClick.AddListener(OnButtonClick_menu);
        cancel_Button.onClick.AddListener(OnButtonClick_cancel);
        speed_Button.onClick.AddListener(OnButtonClick_speed);

        isSpawning_villager = false;
        isSpawning_enemy = false;

        spawnunit = unitTable.spawnquence[unitIndex];

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
        double limit_time = Math.Floor(max_time - Time.time) / max_time;

        timeBar.fillAmount = (float)limit_time;
        Operation();

        if (!isSpawning_villager)
        {
            StartCoroutine(Unit_spawn());
        }

        if (!isSpawning_enemy)
        {
            StartCoroutine(Enemy_spawn());
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject characterInstance;
            Unit movementScript;

            float hp;//ヒットポイント
            int strengh;//攻撃力
            float speed;//素早さ
            float attack_frequency;//攻撃頻度
            float contact_range;//接触範囲
            float attack_scope;//攻撃範囲
            float reaction_rate;//反応速度

            random_value = UnityEngine.Random.Range(0, line_max);

            float line = random_value * 0.3f;

            characterInstance = Instantiate(characterPrefab_second, new Vector3(-10, line, 0), Quaternion.identity);

            movementScript = characterInstance.GetComponent<Unit>();

            hp = 1;
            strengh = 5;
            speed = 3;
            attack_frequency = 3;
            contact_range = 3;
            attack_scope = 7;
            reaction_rate = 1.3f;

            movementScript.Initialize(hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate);
        }
    }

    IEnumerator Unit_spawn()
    {
        isSpawning_villager = true;

        int table_max = unitTable.spawnquence.Count;

        GameObject characterInstance;
        Unit movementScript;

        float hp;//ヒットポイント
        int strengh;//攻撃力
        float speed;//素早さ
        float attack_frequency;//攻撃頻度
        float contact_range;//接触範囲
        float attack_scope;//攻撃範囲
        float reaction_rate;//反応速度

        List<string> features_point;
        List<string> status;

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        unitIndex = UnityEngine.Random.Range(0, table_max);
        spawnunit_after = unitTable.spawnquence[unitIndex];

        //ここで生産
        switch (spawnunit)
        {
            //少女
            case "Unit_1":

                characterInstance = Instantiate(characterPrefab_first, new Vector3(-10, line, 0), Quaternion.identity);

                movementScript = characterInstance.GetComponent<Unit>();

                features_point = new List<string> { "大型BOSSに強い", "中型" };

                status = new List<string> { "攻撃力アップ", "移動速度アップダウン" };

                hp = 5;
                strengh = 1;
                speed = 1;
                attack_frequency = 1;
                contact_range = 1;
                attack_scope = 1;
                reaction_rate = 0;

                movementScript.Initialize(hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate);

                yield return new WaitForSeconds(Interval);

                break;

            //少女（小）
            case "Unit_2":

                characterInstance = Instantiate(characterPrefab_second, new Vector3(-10, line, 0), Quaternion.identity);

                movementScript = characterInstance.GetComponent<Unit>();

                features_point = new List<string> { "大型BOSSに強い", "中型" };

                status = new List<string> { "攻撃力アップ", "移動速度アップダウン" };

                hp = 1;
                strengh = 5;
                speed = 3;
                attack_frequency = 3;
                contact_range = 3;
                attack_scope = 7;
                reaction_rate = 0.3f;

                movementScript.Initialize(hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate);

                yield return new WaitForSeconds(Interval);

                break;

            //バニーガール
            case "Unit_3":

                characterInstance = Instantiate(characterPrefab_third, new Vector3(-10, line, 0), Quaternion.identity);

                movementScript = characterInstance.GetComponent<Unit>();

                features_point = new List<string> { "大型BOSSに強い", "中型" };

                status = new List<string> { "攻撃力アップ", "移動速度アップダウン" };

                hp = 3;
                strengh = 3;
                speed = 5;
                attack_frequency = 5;
                contact_range = 2;
                attack_scope = 5;
                reaction_rate = 0.2f;

                movementScript.Initialize(hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate);

                yield return new WaitForSeconds(Interval);

                break;
        }

        switch (spawnunit_after)
        {
            case "Unit_1":

                yield return new WaitForSeconds(1.8f);

                break;
            case "Unit_2":

                yield return new WaitForSeconds(1.8f);

                break;

            case "Unit_3":

                yield return new WaitForSeconds(0.6f);

                break;
        }
        spawnunit = spawnunit_after;
        isSpawning_villager = false;
    }

    IEnumerator Enemy_spawn()
    {
        isSpawning_enemy = true;

        int table_max = enemyTable.spawnquence_e.Count;

        GameObject characterInstance;
        Enemy movementScript;

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        unitIndex = UnityEngine.Random.Range(0, table_max);
        string spawnunit = enemyTable.spawnquence_e[unitIndex];

        switch (spawnunit)
        {
            case "Enemy":

                characterInstance = Instantiate(enemyPrefab, new Vector3(10, line, 0), Quaternion.identity);

                movementScript = characterInstance.GetComponent<Enemy>();

                movementScript.Initialize(3,3f,1f); //ヒットポイント,攻撃力,ダメージ増減倍率,素早さ,反応速度,攻撃頻度,大きさ,攻撃範囲,かかりやすい状態

                yield return new WaitForSeconds(Interval_e);

                break;
        }

        isSpawning_enemy = false;
    }
}
