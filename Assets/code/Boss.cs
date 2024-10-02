using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private float hp;//HP
    private float strengh;//攻撃力
    private float attack_frequency;//攻撃頻度
    private float attack_scope;//攻撃範囲
    public BossAttackPattern attackPattern;//パターン格納変数
    private int currentAttackIndex = 0;//パターン管理変数
    private bool isPerformingAction = true;

    private HashSet<GameObject> hitAttacks = new HashSet<GameObject>();

    void Start()
    {
        //これらは仮のステータス後でコンストラクタで設定するのでそれを実装したら消す
        hp = 10;
        strengh = 1;
        attack_frequency = 1;
        attack_scope = 5;
    }

    
    void Update()
    {


        // if (attackPattern != null && attackPattern.b_attacksequence.Count > 0)
        // {
        //     StartCoroutine(ExecuteAttacksequence());
        // }
        
        CheckForAttacks();
    }

    private void CheckForAttacks()
    {
        // 敵の周囲に攻撃オブジェクトが存在するかチェック
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 2);
        foreach (Collider2D attackCollider in hitColliders)
        {
            // 攻撃オブジェクトかどうか確認
            if (attackCollider.CompareTag("Attack"))
            {
                GameObject attackObject = attackCollider.gameObject;

                // まだこの攻撃オブジェクトにヒットしていない場合のみ処理を実行
                if (!hitAttacks.Contains(attackObject))
                {
                    this.hp = this.hp - 1;
                    Debug.Log("Boss hp:" + this.hp);

                    // この攻撃オブジェクトを記録して、再度当たり判定が起きないようにする
                    hitAttacks.Add(attackObject);
                }
            }
        }

        if (this.hp <=0)
        {
            Destroy(this.gameObject);
        }
    }


    IEnumerator ExecuteAttacksequence()
    {
        isPerformingAction = true;

        if (attackPattern != null && attackPattern.b_attacksequence.Count > 0)
        {
            // 現在の行動を取得
            string currentAction = attackPattern.b_attacksequence[currentAttackIndex];
            //Debug.Log("Unit is performing: " + currentAction);

            // 行動に応じた処理を実行
            PerformAction(currentAction);

            // 次の行動に進む
            currentAttackIndex = (currentAttackIndex + 1) % attackPattern.b_attacksequence.Count;

            if(currentAction == "Rest"){
                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                yield return new WaitForSeconds(attack_frequency);
            }else if(currentAction == "Attack"){
                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                yield return new WaitForSeconds(5.0f);
            }
        }
        isPerformingAction = false;
    }

    void PerformAction(string action)
    {
        // 行動に基づいてVillagerの動作を実装
        switch (action)
        {
            case "Rest":
                Debug.Log("Unit is resting...");
                // Villagerが歩く処理を実装
                break;

            case "Attack":
                Debug.Log("Unit is attacking...");
                // Villagerが休む処理を実装
                break;

            case "S_Attack":
                Debug.Log("Unit is s_attacking...");
                // Villagerが休む処理を実装
                break;

            default:
                Debug.Log("Unknown action: " + action);
                break;
        }
    }
}
