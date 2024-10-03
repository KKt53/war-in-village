using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_code : MonoBehaviour
{
    public float knockbackStrength = 5f; // ノックバックの強さ
    public float knockbackDuration = 0.5f; // ノックバックの持続時間

    private Vector3 knockbackDirection;
    private float knockbackTimer = 0f; // ノックバックの時間を追跡
    private bool isKnockbackActive = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            isKnockbackActive = true;
        }

        // ノックバックがアクティブかどうかを確認
        if (isKnockbackActive)
        {
            // ノックバックがまだ継続中であれば移動させる
            if (knockbackTimer > 0)
            {
                // 時間に応じて減衰する動きを加える
                float knockbackSpeed = knockbackStrength * (knockbackTimer / knockbackDuration);
                transform.Translate(knockbackDirection * knockbackSpeed * Time.deltaTime);

                // ノックバック時間を減少
                knockbackTimer -= Time.deltaTime;
            }
            else
            {
                // ノックバック終了
                isKnockbackActive = false;
            }
        }
    }

    // ノックバックを開始する関数
    public void ApplyKnockback(Vector3 direction)
    {
        knockbackDirection = direction.normalized; // ノックバックの方向を正規化
        knockbackTimer = knockbackDuration; // ノックバックタイマーをリセット
        isKnockbackActive = true; // ノックバックを有効化
    }
}
