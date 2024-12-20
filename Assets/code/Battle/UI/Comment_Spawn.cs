using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Comment_Spawn : MonoBehaviour
{
    public GameObject canvas;
    public GameObject comment;

    const int line_max = 3;

    private int random_value = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn_comment("ƒeƒXƒg");
        }

    }

    public void Spawn_comment(string comment_text)
    {
        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 30;

        GameObject comment_i;

        comment_i = Instantiate(comment, canvas.transform);

        TMP_Text c_Text = comment_i.GetComponentInChildren<TMP_Text>();

        c_Text.text = comment_text;

        RectTransform rectTransform = comment_i.GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector2(Screen.width / 2, 50 + line);
    }
}
