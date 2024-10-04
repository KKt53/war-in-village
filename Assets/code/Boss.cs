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

    private bool attack_flag;
    private HashSet<GameObject> hitAttacks = new HashSet<GameObject>();
    GameObject AO;//攻撃用オブジェクトインスタンス用変数
    public GameObject Attack_Object;//攻撃用オブジェクト格納用変数

    void Start()
    {
        //これらは仮のステータス後でコンストラクタで設定するのでそれを実装したら消す
        hp = 1000;
        strengh = 1;
        attack_frequency = 2;
        attack_scope = 5;
    }

    
    void Update()
    {

        if (!attack_flag)
        {
            if (attackPattern != null && attackPattern.b_attacksequence.Count > 0)
            {
                //Debug.Log("Attack pattern initialized with " + attackPattern.b_attacksequence.Count + " actions.");
                
            }
            StartCoroutine(ExecuteAttacksequence());
        }

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
                    Attack_Object attack_object = attackObject.GetComponent<Attack_Object>();

                    float damage = attack_object.attack_point;

                    if (attack_object != null)
                    {
                        this.hp = this.hp - attack_object.attack_point;
                    }

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
        attack_flag = true;

        // 現在の行動を取得
        string currentAction = attackPattern.b_attacksequence[currentAttackIndex];
        switch (currentAction)
        {
            case "Rest":
                //Debug.Log("Unit is resting...");
                //Debug.Log(currentAction);
                Destroy(AO);
                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                yield return new WaitForSeconds(10.0f / attack_frequency);

                break;

            case "Attack":
                //Debug.Log("Unit is attacking...");
                //Debug.Log(currentAction);
                AO = Instantiate(Attack_Object, transform.position + new Vector3(attack_scope * -1, 0, 0), Quaternion.identity);
                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                yield return new WaitForSeconds(1.0f);

                break;

            default:
                //Debug.Log("Unknown action: ");
                break;
        }
        
        // 次の行動に進む
        currentAttackIndex = (currentAttackIndex + 1) % attackPattern.b_attacksequence.Count;
        attack_flag = false; // 攻撃後にフラグを解除して再度攻撃可能に
    }
}
