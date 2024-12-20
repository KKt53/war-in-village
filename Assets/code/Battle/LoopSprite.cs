using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopSprite : MonoBehaviour
{
    public Camera mainCamera;
    private Vector2 spriteSize;
    private Vector2 screenBounds;

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteSize = spriteRenderer.bounds.size;

        if (mainCamera == null)
            mainCamera = Camera.main;

        // 画面サイズを計算
        screenBounds = new Vector2(
            mainCamera.orthographicSize * mainCamera.aspect,
            mainCamera.orthographicSize
        );
    }

    void LateUpdate()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 objectPosition = transform.position;

        // 水平方向のループ
        if (cameraPosition.x - objectPosition.x > spriteSize.x)
        {
            objectPosition.x += spriteSize.x * 2f;
        }
        else if (objectPosition.x - cameraPosition.x > spriteSize.x)
        {
            objectPosition.x -= spriteSize.x * 2f;
        }

        

        transform.position = objectPosition;
    }
}
