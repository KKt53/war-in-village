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

    public GameObject characterPrefab_rabbit;
    public GameObject characterPrefab_cat;
    public GameObject characterPrefab_chicken;
    public GameObject characterPrefab_gangi;
    public GameObject characterPrefab_goat;
    public GameObject characterPrefab_napi;
    public GameObject characterPrefab_pig;
    public GameObject characterPrefab_squirrel;

    public GameObject enemyPrefab;

    public GameObject meteorite_place_1;
    public GameObject meteorite_place_2;

    GameObject boss;//ボス用変数

    public GameObject Panel;

    private bool time_switch = false;

    public Button menu_Button;
    public Button cancel_Button;
    public Button speed_Button;

    public GameObject s_button;
    public Sprite speed_off;
    public Sprite speed_on;

    private int speed_switch = 1;

    const int line_max = 2;
    const float Interval = 1.0f;
    const float Interval_e = 1.0f;

    public SpawnUnitTable unitTable;
    public SpawnEnemyTable enemyTable;
    private int unitIndex = 0;
    private int random_value = 0;

    private bool isSpawning_rabbit = false;
    private bool isSpawning_cat = false;
    private bool isSpawning_chicken = false;
    private bool isSpawning_napi = false;
    private bool isSpawning_gangi = false;
    private bool isSpawning_squirrel = false;
    private bool isSpawning_goat = false;
    private bool isSpawning_pig = false;

    private int rabbit_max = 30;
    private int rabbit_count = 0;
    private int cat_max = 30;
    private int cat_count = 0;
    private int chicken_max = 30;
    private int chicken_count = 0;
    private int napi_max = 30;
    private int napi_count = 0;
    private int gangi_max = 30;
    private int gangi_count = 0;
    private int squirrel_max = 30;
    private int squirrel_count = 0;
    private int goat_max = 30;
    private int goat_count = 0;
    private int pig_max = 30;
    private int pig_count = 0;

    private bool isSpawning_enemy = false;

    private string spawnunit;
    private string spawnunit_after;

    private GameObject characterInstance;
    private Unit movementScript;
    private Meteorite meteorite;

    private float hp;//ヒットポイント
    private int strengh;//攻撃力
    private float speed;//素早さ
    private float attack_frequency;//攻撃頻度
    private float contact_range;//接触範囲
    private float attack_scope;//攻撃範囲
    private int reaction_rate_min;//反応速度
    private int reaction_rate_max;//反応速度


    private List<string> characterNames_Rabbit;
    private List<string> characterNames_Cat;
    private List<string> characterNames_Chicken;
    private List<string> characterNames_Gangi;
    private List<string> characterNames_Goat;
    private List<string> characterNames_Napi;
    private List<string> characterNames_Pig;
    private List<string> characterNames_Squirrel;

    private List<string> characterComments_Rabbit;
    private List<string> characterComments_Cat;
    private List<string> characterComments_Chicken;
    private List<string> characterComments_Gangi;
    private List<string> characterComments_Goat;
    private List<string> characterComments_Napi;
    private List<string> characterComments_Pig;
    private List<string> characterComments_Squirrel;

    private List<string> features_point;//ダメージ増減倍率

    // Start is called before the first frame update
    void Start()
    {
        time_switch = false;

        unitIndex = 0;

        menu_Button.onClick.AddListener(OnButtonClick_menu);
        cancel_Button.onClick.AddListener(OnButtonClick_cancel);
        speed_Button.onClick.AddListener(OnButtonClick_speed);

        isSpawning_rabbit = false;
        isSpawning_cat = false;
        isSpawning_chicken = false;
        isSpawning_napi = false;
        isSpawning_gangi = false;
        isSpawning_squirrel = false;
        isSpawning_goat = false;
        isSpawning_pig = false;

        isSpawning_enemy = false;

        spawnunit = unitTable.spawnquence[unitIndex];

        rabbit_max = 30;
        rabbit_count = 0;
        cat_max = 30;
        cat_count = 0;
        chicken_max = 50;
        chicken_count = 0;
        napi_max = 25;
        napi_count = 0;
        gangi_max = 15;
        gangi_count = 0;
        squirrel_max = 30;
        squirrel_count = 0;
        goat_max = 30;
        goat_count = 0;
        pig_max = 10;
        pig_count = 0;

    characterNames_Rabbit = LoadNamesFromJson("Rabbit_name");
        characterNames_Cat = LoadNamesFromJson("Cat_name");
        characterNames_Chicken = LoadNamesFromJson("Chicken_name");
        characterNames_Gangi = LoadNamesFromJson("Gangi_name");
        characterNames_Goat = LoadNamesFromJson("Goat_name");
        characterNames_Napi = LoadNamesFromJson("Napi_name");
        characterNames_Pig = LoadNamesFromJson("Pig_name");
        characterNames_Squirrel = LoadNamesFromJson("Squirrel_name");

        characterComments_Rabbit = LoadNamesFromJson("Rabbit_comment");
        characterComments_Cat = LoadNamesFromJson("Cat_comment");
        characterComments_Chicken = LoadNamesFromJson("Chicken_comment");
        characterComments_Gangi = LoadNamesFromJson("Gangi_comment");
        characterComments_Goat = LoadNamesFromJson("Goat_comment");
        characterComments_Napi = LoadNamesFromJson("Napi_comment");
        characterComments_Pig = LoadNamesFromJson("Pig_comment");
        characterComments_Squirrel = LoadNamesFromJson("Squirrel_comment");
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

        Unit_spawn();

        if (!isSpawning_enemy)
        {
            StartCoroutine(Enemy_spawn());
        }
    }

    private void Operation()
    {

        Image i_s_button = s_button.GetComponent<Image>();

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
            i_s_button.sprite = speed_off;
        }
        //else if (speed_switch == 2)
        //{
        //    Time.timeScale = 1.5f;
        //}
        else if (speed_switch == 2)
        {
            Time.timeScale = 2.0f;
            i_s_button.sprite = speed_on;
        }
        else if (speed_switch >= 3)
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
            //rabbit();
            //cat();
            //chicken();
            //napi();
            //gangi();
            //squirrel();
            //goat();
        }
    }

    private void Unit_spawn()
    {

        //List<string> features_point;
        //List<string> status;

        if (!isSpawning_rabbit)
        {
            if (rabbit_count <= rabbit_max)
            {
                StartCoroutine(rabbit());

                rabbit_count++;
            }
        }

        if (!isSpawning_cat)
        {
            if (cat_count <= cat_max)
            {
                StartCoroutine(cat());

                cat_count++;
            }

        }

        if (!isSpawning_chicken)
        {
            if (chicken_count <= chicken_max)
            {
                StartCoroutine(chicken());

                chicken_count++;
            }

        }

        if (!isSpawning_napi)
        {
            if (napi_count <= napi_max)
            {
                StartCoroutine(napi());

                napi_count++;
            }
        }

        if (!isSpawning_gangi)
        {
            if (gangi_count <= gangi_max)
            {
                StartCoroutine(gangi());

                gangi_count++;
            }
        }

        if (!isSpawning_squirrel)
        {
            if (squirrel_count <= squirrel_max)
            {
                StartCoroutine(squirrel());

                squirrel_count++;
            }
        }

        if (!isSpawning_goat)
        {
            if (goat_count <= goat_max)
            {
                StartCoroutine(goat());

                goat_count++;
            }
        }

        if (!isSpawning_pig)
        {
            if (pig_count <= pig_max)
            {
                StartCoroutine(pig());

                pig_count++;
            }
        }

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

    IEnumerator rabbit()
    {
        isSpawning_rabbit = true;

        string randomName = GetUniqueRandomName(characterNames_Rabbit);

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        characterInstance = Instantiate(characterPrefab_rabbit, new Vector3(-7, line, 0), Quaternion.identity);

        movementScript = characterInstance.GetComponent<Unit>();

        hp = 5;
        strengh = 3;
        speed = 3;
        attack_frequency = 45 / 60;
        contact_range = 2;
        attack_scope = 4;
        reaction_rate_max = 250 / 60;
        reaction_rate_min = 150 / 60;

        movementScript.Initialize("うさぎ", randomName, hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate_max, reaction_rate_min, characterComments_Rabbit);

        yield return new WaitForSeconds(10.0f);

        isSpawning_rabbit = false;
    }

    IEnumerator cat()
    {
        isSpawning_cat = true;

        string randomName = GetUniqueRandomName(characterNames_Cat);

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        characterInstance = Instantiate(characterPrefab_cat, new Vector3(-7, line, 0), Quaternion.identity);

        movementScript = characterInstance.GetComponent<Unit>();

        hp = 4;
        strengh = 3;
        speed = 2;
        attack_frequency = 120 / 60;
        contact_range = 1;
        attack_scope = 4;
        reaction_rate_max = 300 / 60;
        reaction_rate_min = 200 / 60;

        movementScript.Initialize("ねこ", randomName, hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate_max, reaction_rate_min, characterComments_Cat);

        yield return new WaitForSeconds(12.0f);

        isSpawning_cat = false;
    }

    IEnumerator chicken()
    {
        isSpawning_chicken = true;

        string randomName = GetUniqueRandomName(characterNames_Chicken);

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        characterInstance = Instantiate(characterPrefab_chicken, new Vector3(-7, line, 0), Quaternion.identity);

        movementScript = characterInstance.GetComponent<Unit>();

        hp = 1;
        strengh = 1;
        speed = 6;
        attack_frequency = 15 / 60;
        contact_range = 1;
        attack_scope = 2;
        reaction_rate_max = 200 / 60;
        reaction_rate_min = 100 / 60;

        movementScript.Initialize("にわとり", randomName, hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate_max, reaction_rate_min, characterComments_Chicken);

        yield return new WaitForSeconds(6.0f);

        isSpawning_chicken = false;
    }

    IEnumerator napi()
    {
        isSpawning_napi = true;

        string randomName = GetUniqueRandomName(characterNames_Napi);

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        characterInstance = Instantiate(characterPrefab_napi, new Vector3(-7, line, 0), Quaternion.identity);

        movementScript = characterInstance.GetComponent<Unit>();

        hp = 7;
        strengh = 5;
        speed = 4;
        attack_frequency = 50 / 60;
        contact_range = 3;
        attack_scope = 6;
        reaction_rate_max = 200 / 60;
        reaction_rate_min = 150 / 60;

        movementScript.Initialize("なぴ", randomName, hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate_max, reaction_rate_min, characterComments_Napi);

        yield return new WaitForSeconds(1.0f);

        isSpawning_napi = false;
    }

    IEnumerator gangi()
    {
        isSpawning_gangi = true;

        string randomName = GetUniqueRandomName(characterNames_Gangi);

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        characterInstance = Instantiate(characterPrefab_gangi, new Vector3(-7, line, 0), Quaternion.identity);

        movementScript = characterInstance.GetComponent<Unit>();

        hp = 2;
        strengh = 3;
        speed = 2;
        attack_frequency = 45 / 60;
        contact_range = 7;
        attack_scope = 10;
        reaction_rate_max = 500 / 60;
        reaction_rate_min = 300 / 60;

        movementScript.Initialize("ガンギ", randomName, hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate_max, reaction_rate_min, characterComments_Gangi);

        yield return new WaitForSeconds(20.0f);

        isSpawning_gangi = false;
    }

    IEnumerator squirrel()
    {
        isSpawning_squirrel = true;

        string randomName = GetUniqueRandomName(characterNames_Squirrel);

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        characterInstance = Instantiate(characterPrefab_squirrel, new Vector3(-7, line, 0), Quaternion.identity);

        movementScript = characterInstance.GetComponent<Unit>();

        hp = 4;
        strengh = 4;
        speed = 5;
        attack_frequency = 30 / 60;
        contact_range = 2;
        attack_scope = 3;
        reaction_rate_max = 550 / 60;
        reaction_rate_min = 400 / 60;

        movementScript.Initialize("リス", randomName, hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate_max, reaction_rate_min, characterComments_Squirrel);

        yield return new WaitForSeconds(13.0f);

        isSpawning_squirrel = false;
    }

    IEnumerator goat()
    {
        isSpawning_goat = true;

        string randomName = GetUniqueRandomName(characterNames_Goat);

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        characterInstance = Instantiate(characterPrefab_goat, new Vector3(-7, line, 0), Quaternion.identity);

        movementScript = characterInstance.GetComponent<Unit>();

        hp = 8;
        strengh = 2;
        speed = 2;
        attack_frequency = 75 / 60;
        contact_range = 2;
        attack_scope = 4;
        reaction_rate_max = 300 / 60;
        reaction_rate_min = 200 / 60;

        movementScript.Initialize("やぎ", randomName, hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate_max, reaction_rate_min, characterComments_Goat);

        yield return new WaitForSeconds(10.0f);

        isSpawning_goat = false;
    }

    IEnumerator pig()
    {
        isSpawning_pig = true;

        string randomName = GetUniqueRandomName(characterNames_Pig);

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        characterInstance = Instantiate(characterPrefab_pig, new Vector3(-7, line, 0), Quaternion.identity);

        movementScript = characterInstance.GetComponent<Unit>();

        hp = 10;
        strengh = 20;
        speed = 0.5f;
        attack_frequency = 60 / 60;
        contact_range = 2;
        attack_scope = 3;
        reaction_rate_max = 700 / 60;
        reaction_rate_min = 600 / 60;

        movementScript.Initialize("ぶた", randomName, hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate_max, reaction_rate_min, characterComments_Pig);

        yield return new WaitForSeconds(36.0f);

        isSpawning_pig = false;
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
