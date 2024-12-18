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

    GameObject Unit_position;
    GameObject Enemy_position;

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
    private float reaction_rate_min;//反応速度
    private float reaction_rate_max;//反応速度

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

    [SerializeField]
    private List<CharacterData> characterList;

    // Start is called before the first frame update
    void Start()
    {
        unitIndex = 0;
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

        characterList = LoadJsonData();

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

        Unit_position = GameObject.Find("Unit_Position");
        Enemy_position = GameObject.Find("Enemy_Position");
    }

    void Update()
    {
        Operation();

        Unit_spawn();

        //if (!isSpawning_enemy)
        //{
        //    StartCoroutine(Enemy_spawn());
        //}
    }

    private void Operation()
    {
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

    private void us(List<string> characterNames, GameObject characterPrefab, CharacterData cd, string name, List<string> characterComments)
    {
        string randomName = GetUniqueRandomName(characterNames);

        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        characterInstance = Instantiate(characterPrefab, new Vector3(Unit_position.transform.position.x, line, 0), Quaternion.identity);

        movementScript = characterInstance.GetComponent<Unit>();

        hp = cd.hp;
        strengh = cd.strengh;
        speed = cd.speed;
        attack_frequency = cd.attack_frequency;
        contact_range = cd.contact_range;
        attack_scope = cd.attack_scope;
        reaction_rate_max = cd.reaction_rate_max;
        reaction_rate_min = cd.reaction_rate_min;

        movementScript.Initialize(name, randomName, hp, strengh, speed, attack_frequency, contact_range, attack_scope, reaction_rate_max, reaction_rate_min, characterComments);
    }

    IEnumerator rabbit()
    {
        isSpawning_rabbit = true;

        us(characterNames_Rabbit, characterPrefab_rabbit, characterList[0], "うさぎ", characterComments_Rabbit);

        yield return new WaitForSeconds(characterList[0].wait);

        isSpawning_rabbit = false;
    }
    IEnumerator gangi()
    {
        isSpawning_gangi = true;

        us(characterNames_Gangi, characterPrefab_gangi, characterList[1], "ガンギ", characterComments_Gangi);

        yield return new WaitForSeconds(characterList[1].wait);

        isSpawning_gangi = false;
    }

    IEnumerator napi()
    {
        isSpawning_napi = true;

        us(characterNames_Napi, characterPrefab_napi, characterList[2], "なぴ", characterComments_Napi);

        yield return new WaitForSeconds(characterList[2].wait);

        isSpawning_napi = false;
    }

    IEnumerator chicken()
    {
        isSpawning_chicken = true;

        us(characterNames_Chicken, characterPrefab_chicken, characterList[3], "にわとり", characterComments_Chicken);

        yield return new WaitForSeconds(characterList[3].wait);

        isSpawning_chicken = false;
    }

    IEnumerator cat()
    {
        isSpawning_cat = true;

        us(characterNames_Cat, characterPrefab_cat, characterList[4], "ねこ", characterComments_Cat);

        yield return new WaitForSeconds(characterList[4].wait);

        isSpawning_cat = false;
    }

    IEnumerator squirrel()
    {
        isSpawning_squirrel = true;

        us(characterNames_Squirrel, characterPrefab_squirrel, characterList[5], "リス", characterComments_Squirrel);

        yield return new WaitForSeconds(characterList[5].wait);

        isSpawning_squirrel = false;
    }

    IEnumerator goat()
    {
        isSpawning_goat = true;

        us(characterNames_Goat, characterPrefab_goat, characterList[6], "やぎ", characterComments_Goat);

        yield return new WaitForSeconds(characterList[6].wait);

        isSpawning_goat = false;
    }

    IEnumerator pig()
    {
        isSpawning_pig = true;

        us(characterNames_Pig, characterPrefab_pig, characterList[7], "ぶた", characterComments_Pig);

        yield return new WaitForSeconds(characterList[7].wait);

        isSpawning_pig = false;
    }



    private void enemy()
    {
        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 0.3f;

        Enemy movementScript;

        characterInstance = Instantiate(enemyPrefab, new Vector3(Enemy_position.transform.position.x, line, 0), Quaternion.identity);

        movementScript = characterInstance.GetComponent<Enemy>();

        movementScript.Initialize(3, 3f, 1f); //ヒットポイント,攻撃力,ダメージ増減倍率,素早さ,反応速度,攻撃頻度,大きさ,攻撃範囲,かかりやすい状態
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

    private List<CharacterData> LoadJsonData()
    {
        // ResourcesフォルダからJSONを読み込む
        TextAsset jsonFile = Resources.Load<TextAsset>("CharacterData");
        if (jsonFile != null)
        {
            // JSONデータをリストに変換
            List<CharacterData> cd = JsonUtility.FromJson<CharacterDataList>("{\"characters\":" + jsonFile.text + "}").characters;

            return new List<CharacterData>(cd);

            // デバッグで確認
            //foreach (var character in characterList)
            //{
            //    Debug.Log($"hp: {character.hp}, strengh: {character.strengh}, speed: {character.speed}");
            //}
        }
        else
        {
            Debug.LogError("JSONファイルが見つかりません！");
            return new List<CharacterData>();
        }
    }

}
