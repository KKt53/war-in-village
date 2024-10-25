using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    private HashSet<GameObject> hitAttacks = new HashSet<GameObject>();//攻撃重複チェック
    public float hp;//ヒットポイント
    private float strengh;//攻撃力
    private bool isPerformingAction = true;//移動フラグ
    private int direction = 1;//反転方向
    public bool knockback_flag = false;//のけぞりフラグ
    private float speed;//素早さ
    private float attack_frequency;//攻撃頻度
    private float attack_scope = 5f;//攻撃範囲
    private bool attack_flag;//攻撃フラグ

    public UnitAttackPattern attackPattern;//パターン格納変数
    private int currentAttackIndex = 0;//パターン管理変数

    GameObject[] Villager;

    public float jumpHeight = 0.8f;       // ジャンプの高さ
    public float jumpDuration = 0.5f;     // ジャンプにかかる時間
    private float jumpTime = 0f;        // ジャンプの経過時間
    private Vector3 startPosition;      // ジャンプ開始時の位置

    public void Initialize(float c_hp, float c_speed, float c_attack_frequency)
    {
        hp = c_hp;
        speed = c_speed;
        attack_frequency = c_attack_frequency;
    }

    // Start is called before the first frame update
    void Start()
    {
        knockback_flag = false;
        attack_flag = false;
        isPerformingAction = true;
        direction = 1;
        attack_scope = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        Moving();

        CheckForAttacks();

        //ノックバック動作
        if (knockback_flag == true)
        {
            isPerformingAction = false;

            knockback();
        }
    }

    private void Moving()
    {
        // 方向に基づいて移動
        if (isPerformingAction) // この条件が移動制御のためのスイッチ
        {
            transform.Translate(Vector2.left * direction * speed * Time.deltaTime);
        }


        GameObject target = FindNearestAllyInAttackRange();
        if (target != null)
        {
            isPerformingAction = false;
        }
        else
        {
            isPerformingAction = true;
        }

        if (!isPerformingAction && !attack_flag)
        {
            if (attackPattern != null && attackPattern.attacksequence.Count > 0)
            {
                StartCoroutine(ExecuteAttacksequence(target));
            }
        }
    }

    

    //１番近い敵を見つける
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

    IEnumerator ExecuteAttacksequence(GameObject target)
    {
        // 現在の行動を取得
        string currentAction = attackPattern.attacksequence[currentAttackIndex];

        attack_flag = true; // 攻撃後にフラグをオンにする

        switch (currentAction)
        {
            case "Rest":
                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                yield return new WaitForSeconds(10.0f / attack_frequency);

                break;

            case "Attack":

                AttackNearestAllyInRange(target);

                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                yield return new WaitForSeconds(1.0f);

                break;

            default:
                //Debug.Log("Unknown action: ");
                break;
        }

        // 次の行動に進む
        currentAttackIndex = (currentAttackIndex + 1) % attackPattern.attacksequence.Count;
        attack_flag = false; // 攻撃後にフラグを解除して再度攻撃可能に
    }

    //通常単体攻撃
    void AttackNearestAllyInRange(GameObject target)
    {
        
        // ターゲットに攻撃する処理

        Unit unit = target.GetComponent<Unit>();

        unit.hp = unit.hp - 1;
        Debug.Log(unit.hp);
        unit.knockback_flag = true;
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
                GameObject hitObject = attackCollider.gameObject;
                Attack_Object attackObject = hitObject.GetComponent<Attack_Object>();

                // まだこの攻撃オブジェクトにヒットしていない場合のみ処理を実行
                if (!hitAttacks.Contains(hitObject))
                {
                    this.hp = this.hp - attackObject.attack_point;

                    // この攻撃オブジェクトを記録して、再度当たり判定が起きないようにする
                    hitAttacks.Add(hitObject);

                    //★
                    knockback_flag = true;

                }
            }
        }

        if (this.hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void knockback()
    {
        jumpTime += Time.deltaTime;

        // 三角関数を使ってY軸方向の移動を計算
        float progress = jumpTime / jumpDuration; // ジャンプの進捗
        float yOffset = Mathf.Sin(Mathf.PI * progress) * jumpHeight; // sinカーブでY軸の移動量を決定

        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // X軸ののけぞり移動とY軸のジャンプを同時に適用
        transform.position = new Vector2(transform.position.x, startPosition.y + yOffset);

        // ジャンプが終了したかどうかを確認
        if (progress >= 1f)
        {

            // ジャンプ終了時、Y軸位置を元に戻す
            transform.position = new Vector2(transform.position.x, startPosition.y); // Y軸位置をリセット
            knockback_flag = false; // のけぞりフラグを解除
            isPerformingAction = true;
            jumpTime = 0f;
        }
    }
}
