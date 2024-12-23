using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comment_timedelete : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(three_delete());
    }

    IEnumerator three_delete()
    {
        yield return new WaitForSeconds(3.0f);

        Destroy(this.gameObject);
    }
}
