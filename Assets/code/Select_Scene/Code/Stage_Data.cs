using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage_Data
{
    public float left_edge_x;
    public float left_edge_y;

    public float right_edge_x;
    public float right_edge_y;

    public float Unit_spawn_position_x;
    public float Unit_spawn_position_y;

    public float Enemy_spawn_position_x;
    public float Enemy_spawn_position_y;

    public float Camera_position_x;
    public float Camera_position_y;
    public float Camera_position_z;

    public float stage_time;

    public bool enemy_flg;
}

[System.Serializable]
public class Stage_DataList
{
    public List<Stage_Data> stages;
}