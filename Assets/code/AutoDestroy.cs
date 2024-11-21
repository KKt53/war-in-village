using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public void DestroyObject()
    {
        Destroy(gameObject); // オブジェクトを削除
    }
}
