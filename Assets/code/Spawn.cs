using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    //名前は後で変更する
    public GameObject characterPrefab_first;
    public GameObject characterPrefab_second;
    public GameObject characterPrefab_third;



    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        GameObject characterInstance;
        Unit movementScript;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            characterInstance = Instantiate(characterPrefab_first, new Vector3(-10, 1, 0), Quaternion.identity);

            movementScript = characterInstance.GetComponent<Unit>();

            movementScript.Initialize(1, 5f, 0.5f, 5.0f, 1, 4); //攻撃力,素早さ,反応速度,攻撃頻度,大きさ,攻撃範囲
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            characterInstance = Instantiate(characterPrefab_second, new Vector3(-10, 1, 0), Quaternion.identity);

            movementScript = characterInstance.GetComponent<Unit>();

            movementScript.Initialize(1, 2f, 0.8f, 1.0f, 1, 5);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            characterInstance = Instantiate(characterPrefab_third, new Vector3(-10, 1, 0), Quaternion.identity);

            movementScript = characterInstance.GetComponent<Unit>();

            movementScript.Initialize(1, 8f, 0.1f, 10.0f, 1, 3);
        }
    }
}
