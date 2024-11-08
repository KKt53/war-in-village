using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

[System.Serializable]
public class NameList
{
    public List<string> names;
}

public class Spawn : MonoBehaviour
{
    //名前は後で変更する
    public GameObject characterPrefab_first;
    public GameObject characterPrefab_second;
    public GameObject characterPrefab_third;

    public GameObject characterPrefab_prot;

    public GameObject enemyPrefab;

    GameObject boss;//ボス用変数

    public GameObject Panel;

    private bool time_switch = false;

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

    private GameObject characterInstance;
    private Unit movementScript;

    private float hp;//ヒットポイント
    private int strengh;//攻撃力
    private float speed;//素早さ
    private float attack_frequency;//攻撃頻度
    private float contact_range;//接触範囲
    private float attack_scope;//攻撃範囲
    private float reaction_rate;//反応速度

    private List<string> characterNames_Unit1;
    private List<string> characterNames_Unit2;
    private List<string> characterNames_Unit3;

    // Start is called before the first frame update
    void Start()
    {
        time_switch = false;

        unitIndex = 0;

        menu_Button.onClick.AddListener(OnButtonClick_menu);
        cancel_Button.onClick.AddListener(OnButtonClick_cancel);
        speed_Button.onClick.AddListener(OnButtonClick_speed);

        isSpawning_villager = false;
        isSpawning_enemy = false;

        spawnunit = unitTable.spawnquence[unitIndex];

        characterNames_Unit1 = LoadNamesFromJson("Unit1_name");
        characterNames_Unit2 = LoadNamesFromJson("Unit2_name");
        characterNames_Unit3 = LoadNamesFromJson("Unit3_name");
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
            //spawn_2();

            //隕石コード消すな
            //characterInstance = Instantiate(characterPrefab_prot, new Vector3(2, 7, 0), Quaternion.identity);
        }
    }

    IEnumerator Unit_spawn()
    {
        isSpawning_villager = true;

        int table_max = unitTable.spawnquence.Count;

        

        //List<string> features_point;
        //List<string> status;

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        unitIndex = UnityEngine.Random.Range(0, table_max);
        spawnunit_after = unitTable.spawnquence[unitIndex];

        //ここで生産
        switch (spawnunit)
        {
            //少女
            case "Unit_1":

                spawn_1();

                yield return new WaitForSeconds(Interval);

                break;

            //少女（小）
            case "Unit_2":

                spawn_2();

                yield return new WaitForSeconds(Interval);

                break;

            //バニーガール
            case "Unit_3":

                spawn_3();

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

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        unitIndex = UnityEngine.Random.Range(0, table_max);
        string spawnunit = enemyTable.spawnquence_e[unitIndex];

        switch (spawnunit)
        {
            case "Enemy":

                enemy();

                yield return new WaitForSeconds(Interval_e);

                break;
        }

        isSpawning_enemy = false;
    }

    private void spawn_1()
    {
        string randomName = GetUniqueRandomName(characterNames_Unit1);

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        characterInstance = Instantiate(characterPrefab_first, new Vector3(-7, line, 0), Quaternion.identity);

        movementScript = characterInstance.GetComponent<Unit>();

        hp = 5;
        strengh = 1;
        speed = 1;
        attack_frequency = 1;
        contact_range = 1;
        attack_scope = 1;
        reaction_rate = 0;

        movementScript.Initialize("Unit1", randomName, hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate);
    }

    private void spawn_2()
    {
        string randomName = GetUniqueRandomName(characterNames_Unit2);

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        characterInstance = Instantiate(characterPrefab_second, new Vector3(-7, line, 0), Quaternion.identity);

        movementScript = characterInstance.GetComponent<Unit>();

        hp = 1;
        strengh = 5;
        speed = 3;
        attack_frequency = 3;
        contact_range = 3;
        attack_scope = 7;
        reaction_rate = 0.3f;

        movementScript.Initialize("Unit2", randomName, hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate);
    }

    private void spawn_3()
    {
        string randomName = GetUniqueRandomName(characterNames_Unit3);

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        characterInstance = Instantiate(characterPrefab_third, new Vector3(-7, line, 0), Quaternion.identity);

        movementScript = characterInstance.GetComponent<Unit>();

        hp = 3;
        strengh = 3;
        speed = 5;
        attack_frequency = 5;
        contact_range = 2;
        attack_scope = 5;
        reaction_rate = 0.2f;

        movementScript.Initialize("Unit3", randomName, hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate);
    }

    private void enemy()
    {
        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        Enemy movementScript;

        characterInstance = Instantiate(enemyPrefab, new Vector3(7, line, 0), Quaternion.identity);

        movementScript = characterInstance.GetComponent<Enemy>();

        movementScript.Initialize(3, 3f, 1f); //ヒットポイント,攻撃力,ダメージ増減倍率,素早さ,反応速度,攻撃頻度,大きさ,攻撃範囲,かかりやすい状態
    }

    private List<string> LoadNamesFromJson(string file)
    {
        TextAsset jsonTextFile = Resources.Load<TextAsset>(file);
        if (jsonTextFile != null)
        {
            NameList nameList = JsonUtility.FromJson<NameList>(jsonTextFile.text);
            return new List<string>(nameList.names);
        }
        else
        {
            Debug.LogWarning("JSON file not found!");
            return new List<string>();
        }
    }

    public string GetUniqueRandomName(List<string> characterNames)
    {
        if (characterNames.Count == 0)
        {
            Debug.LogWarning("No more unique names available.");
            return null;
        }

        int index = UnityEngine.Random.Range(0, characterNames.Count);
        string selectedName = characterNames[index];
        characterNames.RemoveAt(index);

        return selectedName;
    }
}
