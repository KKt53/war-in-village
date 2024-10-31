using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : MonoBehaviour, IAttackable
{
    private int hp_max;
    public int hp { get; set; }//HP
    private float strengh;//攻撃力
    private float attack_frequency;//攻撃頻度
    private float attack_scope;//攻撃範囲
    //private List<string> features_point;//ダメージ増減倍率
    private int Level_max;
    private int Level;//レベル
    private float experience;//経験値
    private float experience_reference;//経験値

    public BossAttackPattern attackPattern;//パターン格納変数
    private int currentAttackIndex = 0;//パターン管理変数

    private bool attack_flag;
    private HashSet<GameObject> hitAttacks = new HashSet<GameObject>();
    GameObject AO;//攻撃用オブジェクトインスタンス用変数
    public GameObject Attack_Object;//攻撃用オブジェクト格納用変数

    GameObject[] Villager;

    void Start()
    {
        //これらは仮のステータス後でコンストラクタで設定するのでそれを実装したら消す
        hp = 1000;
        hp_max = hp;
        strengh = 1;
        attack_frequency = 2;
        attack_scope = 5;
        Level = 1;
        Level_max = 3;
        experience = 0;
        experience_reference = 3;
        //features_point = new List<string> { "大型BOSSに強い", "中型" };
    }

    //public void Initialize(int c_hp, int c_strengh, float c_attack_frequency, float c_attack_scope)
    //{
    //    hp = c_hp;
        
    //    strengh = c_strengh;
    //    attack_frequency = c_attack_frequency;
    //    attack_scope = c_attack_scope;
    //}


    void Update()
    {

        if (!attack_flag)
        {
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

                    int damage = attack_object.attack_point;

                    if (attack_object != null)
                    {
                        this.hp = this.hp - attack_object.attack_point;
                    }

                    //Debug.Log("Boss hp:" + this.hp);

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

        int random_value = UnityEngine.Random.Range(0, 2);

        // 現在の行動を取得
        string currentAction = attackPattern.b_attacksequence[currentAttackIndex];
        switch (currentAction)
        {
            case "Rest":
                Destroy(AO);
                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                yield return new WaitForSeconds(10.0f / attack_frequency);

                break;

            case "Attack":

                if (random_value == 0)
                {
                    AttackNearestAllyInRange();
                }
                else if (random_value == 1)
                {

                }


                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                yield return new WaitForSeconds(1.0f);

                break;

            case "S_Attack":

                if (Level != 1)
                {
                    AO = Instantiate(Attack_Object, transform.position + new Vector3(attack_scope * -1, 0, 0), Quaternion.identity);
                    // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                    yield return new WaitForSeconds(1.0f);
                }

                break;

            default:
                //Debug.Log("Unknown action: ");
                break;
        }
        
        // 次の行動に進む
        currentAttackIndex = (currentAttackIndex + 1) % attackPattern.b_attacksequence.Count;
        attack_flag = false; // 攻撃後にフラグを解除して再度攻撃可能に
    }

    //ボスから１番近い敵を見つける
    GameObject FindNearestAllyInAttackRange()
    {
        GameObject nearestAlly = null;
        float shortestDistance = Mathf.Infinity;

        Villager = GameObject.FindGameObjectsWithTag("Villager");

        foreach (GameObject ally in Villager)
        {
            // ボスと味方ユニット間の距離を計算
            float distance = Vector2.Distance(transform.position, ally.transform.position);
            
            // ユニットが攻撃範囲内にあるか確認
            if (distance <= attack_scope && distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestAlly = ally;
            }
        }

        return nearestAlly;
    }

    public void ApplyDamage(int damage)
    {
        hp -= damage;
    }

    //ボスの通常攻撃
    void AttackNearestAllyInRange()
    {
        GameObject target = FindNearestAllyInAttackRange();

        //Debug.Log(target);
        if (target != null)
        {
            // ターゲットに攻撃する処理

            Unit unit = target.GetComponent<Unit>();

            unit.hp = unit.hp - strengh;
            unit.knockback_flag = true;

            experience++;
            
            if(experience >= experience_reference && Level < Level_max)
            {
                Level++;
                experience_reference = experience_reference * 1.2f;
                strengh = strengh * 1.2f;
                experience = 0;
                Debug.Log("Level: " + Level);
            }
        }
    }
}
