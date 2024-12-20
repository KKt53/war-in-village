using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;

public class Level_Up : MonoBehaviour
{
    GameObject boss;//É{ÉXópïœêî

    public Sprite number_1;
    public Sprite number_2;
    public Sprite number_3;
    public Sprite number_4;
    public Sprite number_5;
    public Sprite number_6;
    public Sprite number_7;
    public Sprite number_8;
    public Sprite number_9;
    public Sprite number_10;
    public Sprite number_11;
    public Sprite number_12;
    public Sprite number_13;
    public Sprite number_14;
    public Sprite number_15;


    // Start is called before the first frame update
    void OnEnable()
    {
        boss = GameObject.Find("Boss");

        Boss boss_i = boss.GetComponent<Boss>();

        Transform child = transform.Find("Figure");

        Image imageComponent = child.GetComponent<Image>();

        switch (boss_i.Level)
        {
            case 1:

                imageComponent.sprite = number_1;

                break;

            case 2:

                imageComponent.sprite = number_2;

                break;

            case 3:

                imageComponent.sprite = number_3;

                break;

            case 4:

                imageComponent.sprite = number_4;

                break;

            case 5:

                imageComponent.sprite = number_5;

                break;

            case 6:

                imageComponent.sprite = number_6;

                break;

            case 7:

                imageComponent.sprite = number_7;

                break;

            case 8:

                imageComponent.sprite = number_8;

                break;

            case 9:

                imageComponent.sprite = number_9;

                break;

            case 10:

                imageComponent.sprite = number_10;

                break;

            case 11:

                imageComponent.sprite = number_11;

                break;

            case 12:

                imageComponent.sprite = number_12;

                break;

            case 13:

                imageComponent.sprite = number_13;

                break;

            case 14:

                imageComponent.sprite = number_14;

                break;

            case 15:

                imageComponent.sprite = number_15;

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Display_Level());
    }

    IEnumerator Display_Level()
    {
        yield return new WaitForSeconds(1.0f);
        this.gameObject.SetActive(false);
    }
}
