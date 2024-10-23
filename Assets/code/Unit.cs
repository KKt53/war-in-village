using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEditor;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;
using static UnityEngine.EventSystems.EventTrigger;

public class Unit : MonoBehaviour
{
    public float hp;//ヒットポイント
    private float strengh;//攻撃力
    private List<string> features_point;//ダメージ増減倍率
    private float speed;//素早さ
    private float reaction_rate;//反応速度
    private float attack_frequency;//攻撃頻度
    private float size;//大きさ
    private float attack_scope;//攻撃範囲
    private List<string> status;//かかりやすい状態

    private int direction = 1;//反転方向
    public bool knockback_flag = false;//のけぞりフラグ
    private bool attack_flag;//攻撃フラグ
    private bool isPerformingAction = true;//移動フラグ
    private float doublePressTime = 2.0f;      // 連打とみなす時間間隔（秒）
    private float lastPressTime_l = 0f; // 前回キーが押された時間(左)
    private float lastPressTime_r = 0f; // 前回キーが押された時間(右)
    private bool movement_disabled_flag = false;
    private bool movement_r_flag = true;
    private bool movement_l_flag = false;

    public UnitAttackPattern attackPattern;//パターン格納変数
    private int currentAttackIndex = 0;//パターン管理変数

    private HashSet<GameObject> hitAttacks = new HashSet<GameObject>();//攻撃重複チェック
    private SpriteRenderer spriteRenderer;//画像格納用変数
    GameObject boss;//ボス用変数
    GameObject AO;//攻撃用オブジェクトインスタンス用変数
    public GameObject Attack_Object;//攻撃用オブジェクト格納用変数
    SpriteRenderer sr;//画像格納用インスタンス
    Attack_Object AO_I;//攻撃オブジェクト格納用インスタンス

    public float jumpHeight = 0.8f;       // ジャンプの高さ
    public float jumpDuration = 0.5f;     // ジャンプにかかる時間
    private bool isJumping = false;     // ジャンプ中かどうか
    private float jumpTime = 0f;        // ジャンプの経過時間
    private Vector3 startPosition;      // ジャンプ開始時の位置

    private bool moving_standby = false; //移動待機フラグ

    private Vector2 pointA = new Vector2(-10f, 0f); // 地点Aの座標

    public void Initialize(float c_hp, float c_strengh, List<string> c_features_point, float c_speed, float c_reaction_rate, float c_attack_frequency, float c_size, float c_attack_scope, List<string> c_status)
    {
        hp = c_hp;
        strengh = c_strengh;
        features_point = c_features_point;
        speed = c_speed;
        reaction_rate = c_reaction_rate;
        attack_frequency = c_attack_frequency;
        size = c_size;
        attack_scope = c_attack_scope;
        status = c_status;
    }

    void Start()
    {
        moving_standby = false;
        isJumping = false;
        direction = 1;
        direction = 1;
        knockback_flag = false;
        isPerformingAction = true;
        boss = GameObject.FindWithTag("Boss");
        attack_flag = false;
        this.sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Moving();

        CheckForAttacks();

        Vector2 position = transform.position;

        // x座標とy座標の範囲をClampで制限
        position.x = Mathf.Clamp(position.x, Mathf.Min(pointA.x, boss.transform.position.x), Mathf.Max(pointA.x, boss.transform.position.x));

        // 位置を更新
        transform.position = position;

        //★
        //ノックバック動作
        if (knockback_flag == true)
        {
            isPerformingAction = false;

            knockback();
        }
    }
    private void OnDestroy()
    {
        Destroy(AO);
    }

    private void Moving()
    {
        //攻撃してる最中じゃなかったら
        if (!movement_disabled_flag)
        {
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // 現在の時間を取得
                float currentTime = Time.time;

                float total = currentTime - lastPressTime_l;


                // 前回の押下からの経過時間が設定した連打時間内であれば
                if (total >= doublePressTime || direction == 1)
                {
                    StartCoroutine(ChangeDirectionWithDelay_left());
                }

                lastPressTime_l = currentTime;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // 現在の時間を取得
                float currentTime = Time.time;

                float total = currentTime - lastPressTime_r;


                // 前回の押下からの経過時間が設定した連打時間内であれば
                if (total >= doublePressTime || direction == -1)
                {
                    StartCoroutine(ChangeDirectionWithDelay_right());
                }


                lastPressTime_r = currentTime;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                movement_r_flag = false;
                movement_l_flag = true;

                //待機フラグON
                moving_standby = true;
                Debug.Log("left standby");
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                movement_r_flag = true;
                movement_l_flag = false;

                //待機フラグON
                moving_standby = true;
                Debug.Log("right standby");
            }
         }

        

        // 方向に基づいて移動
        if (isPerformingAction) // この条件が移動制御のためのスイッチ
        {
            transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
        }

        if (!isPerformingAction && !attack_flag)
        {
            if (attackPattern != null && attackPattern.attacksequence.Count > 0)
            {
                StartCoroutine(ExecuteAttacksequence());
            }
        }

        if (boss)
        {
            if (transform.position.x + attack_scope < boss.transform.position.x)
            {
                
                isPerformingAction = true;
            }
            else
            {
                if (direction == -1)
                {
                    isPerformingAction = true;
                }
                else
                {
                    isPerformingAction = false;
                }
            }
        }
        else
        {
            Destroy(AO);
            attack_flag = false;
            isPerformingAction = true;
        }

        if (isJumping == true)
        {
            Jumping();
        }

    }

    private void CheckForAttacks()
    {
        // 敵の周囲に攻撃オブジェクトが存在するかチェック
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 2);
        foreach (Collider2D attackCollider in hitColliders)
        {
            // 攻撃オブジェクトかどうか確認
            if (attackCollider.CompareTag("B_Attack"))
            {
                GameObject attackObject = attackCollider.gameObject;

                // まだこの攻撃オブジェクトにヒットしていない場合のみ処理を実行
                if (!hitAttacks.Contains(attackObject))
                {
                    this.hp = this.hp - 1;
                    
                    // この攻撃オブジェクトを記録して、再度当たり判定が起きないようにする
                    hitAttacks.Add(attackObject);

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


    IEnumerator ExecuteAttacksequence()
    {
        // 現在の行動を取得
        string currentAction = attackPattern.attacksequence[currentAttackIndex];

        attack_flag = true; // 攻撃後にフラグをオンにする
        movement_disabled_flag = true;

        switch (currentAction)
        {
            case "Rest":
                Destroy(AO);
                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                yield return new WaitForSeconds(10.0f / attack_frequency);

                break;

            case "Attack":
                AO = Instantiate(Attack_Object, transform.position + new Vector3(attack_scope, 0, 0), Quaternion.identity);

                AO_I = AO.GetComponent<Attack_Object>();

                AO_I.Initialize(strengh, features_point);

                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                yield return new WaitForSeconds(1.0f);
                movement_disabled_flag = false;

                if (moving_standby == true)
                {
                    if (movement_r_flag == true && movement_l_flag == false)
                    {
                        StartCoroutine(ChangeDirectionWithDelay_right());
                    }
                    else if (movement_l_flag == true && movement_r_flag == false)
                    {
                        StartCoroutine(ChangeDirectionWithDelay_left());
                    }
                }

                break;

            default:
                Debug.Log("Unknown action: ");
                break;
        }

        // 次の行動に進む
        currentAttackIndex = (currentAttackIndex + 1) % attackPattern.attacksequence.Count;
        attack_flag = false; // 攻撃後にフラグを解除して再度攻撃可能に
        movement_disabled_flag = false;
    }

    IEnumerator ChangeDirectionWithDelay_left()
    {
        yield return new WaitForSeconds(reaction_rate);

        jumpTime = 0f;
        startPosition = transform.position; // ジャンプ開始時の位置を保存

        sr.flipX = false;
        direction = -1;//左

        // ジャンプ開始
        isJumping = true;

        if (!isPerformingAction)
        {
            Destroy(AO);
            moving_standby = false;
            isPerformingAction = true; // 移動を再開
        }
    }
    IEnumerator ChangeDirectionWithDelay_right()
    {
        yield return new WaitForSeconds(reaction_rate);

        jumpTime = 0f;
        startPosition = transform.position; // ジャンプ開始時の位置を保存

        sr.flipX = true;
        direction = 1;//右

        // ジャンプ開始
        isJumping = true;

        if (!isPerformingAction)
        {
            Destroy(AO);
            moving_standby = false;
            isPerformingAction = true; // 移動を再開
            
        }
    }

    private void Jumping()
    {
        jumpTime += Time.deltaTime;

        // 三角関数を使ってY軸方向の移動を計算
        float progress = jumpTime / jumpDuration; // ジャンプの進捗
        float yOffset = Mathf.Sin(Mathf.PI * progress) * jumpHeight; // sinカーブでY軸の移動量を決定

        // Y軸にジャンプの高さを加える
        transform.position = new Vector2(transform.position.x, startPosition.y + yOffset);

        

        // ジャンプが終了したかどうかを確認
        if (progress >= 1f)
        {
            isJumping = false; // ジャンプ終了
            //transform.position = new Vector2(transform.position.x, 0.0f); // 元の位置に戻す
        }

    }

    private void knockback()
    {
        jumpTime += Time.deltaTime;

        // 三角関数を使ってY軸方向の移動を計算
        float progress = jumpTime / jumpDuration; // ジャンプの進捗
        float yOffset = Mathf.Sin(Mathf.PI * progress) * jumpHeight; // sinカーブでY軸の移動量を決定

        transform.Translate(Vector2.right * direction * speed * -1 * Time.deltaTime);

        // X軸ののけぞり移動とY軸のジャンプを同時に適用
        transform.position = new Vector2(transform.position.x, startPosition.y + yOffset);

        // ジャンプが終了したかどうかを確認
        if (progress >= 1f)
        {
            Debug.Log("knocked off!");
            // ジャンプ終了時、Y軸位置を元に戻す
            transform.position = new Vector2(transform.position.x, startPosition.y); // Y軸位置をリセット
            knockback_flag = false; // のけぞりフラグを解除
            isPerformingAction = true;
            jumpTime = 0f;
        }
    }
}
