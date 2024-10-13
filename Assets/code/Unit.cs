using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
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
    private bool direction_flag = false;//反転フラグ
    public bool knockback_flag = false;//のけぞりフラグ

    private bool attack_flag;
    private HashSet<GameObject> hitAttacks = new HashSet<GameObject>();

    private SpriteRenderer spriteRenderer;

    public UnitAttackPattern attackPattern;//パターン格納変数
    private int currentAttackIndex = 0;//パターン管理変数
    private bool isPerformingAction = true;
    GameObject boss;//ボス用変数
    GameObject AO;//攻撃用オブジェクトインスタンス用変数
    public GameObject Attack_Object;//攻撃用オブジェクト格納用変数
    SpriteRenderer sr;//画像格納用変数

    Attack_Object AO_I;

    private float doublePressTime = 2.0f;      // 連打とみなす時間間隔（秒）
    private float lastPressTime_l = 0f; // 前回キーが押された時間(左)
    private float lastPressTime_r = 0f; // 前回キーが押された時間(右)

    public float jumpHeight = 0.8f;       // ジャンプの高さ
    public float jumpDuration = 0.5f;     // ジャンプにかかる時間
    private bool isJumping = false;     // ジャンプ中かどうか
    private float jumpTime = 0f;        // ジャンプの経過時間
    private Vector3 startPosition;      // ジャンプ開始時の位置

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
        direction = 1;
        direction_flag = false;
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

        if (isJumping)
        {
            Jumping();
        }

        if (knockback_flag)
        {
            isPerformingAction = false;

            transform.Translate(Vector2.right * direction * speed * -1 * Time.deltaTime);

            Knockback();
        }
    }
    private void OnDestroy()
    {
        Destroy(AO);
    }

    private void Moving()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // 現在の時間を取得
            float currentTime = Time.time;

            float total = currentTime - lastPressTime_l;

            // 前回の押下からの経過時間が設定した連打時間内であれば
            if (total >= doublePressTime || direction_flag == false)
            {
                Debug.Log("total_l:" + total);

                jumpTime = 0f;
                startPosition = transform.position; // ジャンプ開始時の位置を保存

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
            if (total >= doublePressTime || direction_flag == true) {

                Debug.Log("total_r:" + total);

                jumpTime = 0f;
                startPosition = transform.position; // ジャンプ開始時の位置を保存

                StartCoroutine(ChangeDirectionWithDelay_right());
            }


            lastPressTime_r = currentTime;
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
                attack_flag = true; // 攻撃後にフラグをオンにする
            }
        }

        if (knockback_flag == false)
        {
            if (direction_flag == false)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
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
                isPerformingAction = false;
            }
        }
        else
        {
            Destroy(AO);
            attack_flag = false;
            isPerformingAction = true;
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

        switch (currentAction)
        {
            case "Rest":
                Destroy(AO);
                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                yield return new WaitForSeconds(10.0f / attack_frequency);

                break;

            case "Attack":
                AO = Instantiate(Attack_Object, transform.position + new Vector3(attack_scope, 1, 0), Quaternion.identity);

                AO_I = AO.GetComponent<Attack_Object>();

                AO_I.Initialize(strengh, features_point);

                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                yield return new WaitForSeconds(1.0f);

                break;

            default:
                Debug.Log("Unknown action: ");
                break;
        }

        // 次の行動に進む
        currentAttackIndex = (currentAttackIndex + 1) % attackPattern.attacksequence.Count;
        attack_flag = false; // 攻撃後にフラグを解除して再度攻撃可能に
    }


    IEnumerator ChangeDirectionWithDelay_left()
    {
        yield return new WaitForSeconds(reaction_rate);

        sr.flipX = false;
        direction_flag = true; // 方向を即時変更

        // ジャンプ開始
        isJumping = true;

        if (!isPerformingAction)
        {
            Destroy(AO);
            isPerformingAction = true; // 移動を再開
        }
    }
    IEnumerator ChangeDirectionWithDelay_right()
    {
        yield return new WaitForSeconds(reaction_rate);

        sr.flipX = true;
        direction_flag = false; // 方向を即時変更

        // ジャンプ開始
        isJumping = true;

        if (!isPerformingAction)
        {
            Destroy(AO);
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
        transform.position = new Vector2(transform.position.x, 0 + yOffset);

        // ジャンプが終了したかどうかを確認
        if (progress >= 1f)
        {
            isJumping = false; // ジャンプ終了
            transform.position = new Vector2(transform.position.x, 0); // 元の位置に戻す
        }
        
    }

    private void Knockback()
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
            knockback_flag = false;
            
            transform.position = new Vector2(transform.position.x, startPosition.y); // 元の位置に戻す
        }

        if (startPosition.y == transform.position.y)
        {
            Debug.Log("knockback flag offed!");
            isPerformingAction = true;
        }

    }
}
