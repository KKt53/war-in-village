using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Position : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Select_Push_1.push_flg == true)
        {
            this.transform.position = Select_Push_1.Unit_spawn_position;
        }

            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
