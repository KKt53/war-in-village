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
            // �L�����N�^�[�̃C���X�^���X�𐶐�
            GameObject characterInstance = Instantiate(characterPrefab, new Vector3(-10, 1, 0), Quaternion.identity);

            // ���x��ݒ� (��: 5f)
            Villager movementScript = characterInstance.GetComponent<Villager>();
            movementScript.Initialize(1, 5f, 1, 1, 1, 5);  // ���x��5�ɐݒ�
        }

        
    }

}
