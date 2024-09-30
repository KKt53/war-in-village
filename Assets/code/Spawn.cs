using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject characterPrefab;

    

    // Start is called before the first frame update
    void Start()
    {
        

        
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject characterInstance = Instantiate(characterPrefab, new Vector3(-10, 1, 0), Quaternion.identity);

            Unit movementScript = characterInstance.GetComponent<Unit>();
            movementScript.Initialize(1, 5f, 0.1f, 10.0f, 1, 5); //攻撃力,素早さ,反応速度,攻撃頻度,大きさ,攻撃範囲
        }

        
    }

}
