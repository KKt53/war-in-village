using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Select_Push_1 : MonoBehaviour
{
    private GameObject stage1;
    private GameObject stage2;

    private Button button1;
    private Button button2;

    public static Vector2 left_edge;
    public static Vector2 right_edge;

    public static Vector2 Unit_spawn_position;
    public static Vector2 Enemy_spawn_position;

    public static Vector3 Camera_position;

    public static float stage_time;

    public static bool enemy_flg;

    public static bool push_flg = false;

    [SerializeField]
    private List<Stage_Data> stageList;

    // Start is called before the first frame update
    void Start()
    {
        stage1 = GameObject.Find("ステージ１");
        stage2 = GameObject.Find("ステージ２");

        button1 = stage1.GetComponent<Button>();
        button2 = stage2.GetComponent<Button>();

        button1.onClick.AddListener(OnButtonClick_event_1);
        button2.onClick.AddListener(OnButtonClick_event_2);

        stageList = LoadJsonData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnButtonClick_event_1()
    {
        Debug.Log("clicked");

        left_edge = new Vector2(stageList[0].left_edge_x, stageList[0].left_edge_y);

        right_edge = new Vector2(stageList[0].right_edge_x, stageList[0].right_edge_y);

        Unit_spawn_position = new Vector2(stageList[0].Unit_spawn_position_x, stageList[0].Unit_spawn_position_y);

        Enemy_spawn_position = new Vector2(stageList[0].Enemy_spawn_position_x, stageList[0].Enemy_spawn_position_y);

        Camera_position = new Vector3(stageList[0].Camera_position_x, stageList[0].Camera_position_y, stageList[0].Camera_position_z);

        stage_time = stageList[0].stage_time;

        enemy_flg = stageList[0].enemy_flg;

        push_flg = true;

        SceneManager.LoadScene("Battle");

    }

    void OnButtonClick_event_2()
    {
        left_edge = new Vector2(stageList[1].left_edge_x, stageList[1].left_edge_y);

        right_edge = new Vector2(stageList[1].right_edge_x, stageList[1].right_edge_y);

        Unit_spawn_position = new Vector2(stageList[1].Unit_spawn_position_x, stageList[1].Unit_spawn_position_y);

        Enemy_spawn_position = new Vector2(stageList[1].Enemy_spawn_position_x, stageList[1].Enemy_spawn_position_y);

        Camera_position = new Vector3(stageList[1].Camera_position_x, stageList[1].Camera_position_y, stageList[1].Camera_position_z);

        stage_time = stageList[1].stage_time;

        enemy_flg = stageList[1].enemy_flg;

        push_flg = true;

        SceneManager.LoadScene("Battle");
    }

    private List<Stage_Data> LoadJsonData()
    {
        // ResourcesフォルダからJSONを読み込む
        TextAsset jsonFile = Resources.Load<TextAsset>("StageData");
        if (jsonFile != null)
        {
            // JSONデータをリストに変換
            List<Stage_Data> cd = JsonUtility.FromJson<Stage_DataList>("{\"stages\":" + jsonFile.text + "}").stages;

            return new List<Stage_Data>(cd);

        }
        else
        {
            Debug.LogError("JSONファイルが見つかりません！");
            return new List<Stage_Data>();
        }
    }
}