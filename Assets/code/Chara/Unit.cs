using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Collections.Specialized.BitVector32;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public string type;
    public string name_of_death;
    public float hp;//ヒットポイント
    private float hp_max;
    private int strengh;//攻撃力
    private float speed;//素早さ
    private float attack_frequency;//攻撃頻度
    private float contact_range;//接触範囲
    private float attack_scope;//攻撃範囲
    private int reaction_rate_min;//反応速度
    private int reaction_rate_max;//反応速度
    public List<string> comments;

    private List<string> features_point;//ダメージ増減倍率
    private List<string> status;//かかりやすい状態

    private int direction = 1;//反転方向
    public bool knockback_flag = false;//のけぞりフラグ

    private bool attack_flag = false;//攻撃フラグ

    private bool isPerformingAction = true;//移動フラグ
    private float doublePressTime = 2.0f;      // 連打とみなす時間間隔（秒）
    private float lastPressTime_l = 0f; // 前回キーが押された時間(左)
    private float lastPressTime_r = 0f; // 前回キーが押された時間(右)
    private bool movement_disabled_flag = false;//動けないフラグ
    private bool movement_r_flag = true;//右反転フラグ
    private bool movement_l_flag = false;//左反転フラグ

    public UnitAttackPattern attackPattern;//パターン格納変数
    private int currentAttackIndex = 0;//パターン管理変数

    private HashSet<GameObject> hitAttacks = new HashSet<GameObject>();//攻撃重複チェック
    private SpriteRenderer spriteRenderer;//画像格納用変数
    GameObject boss;//ボス用変数
    GameObject AO;//攻撃用オブジェクトインスタンス用変数
    public GameObject attack_Object;//攻撃用オブジェクト格納用変数
    SpriteRenderer sr;//画像格納用インスタンス

    public float jumpHeight = 0.8f;       // ジャンプの高さ
    public float jumpDuration = 0.5f;     // ジャンプにかかる時間
    private bool isJumping = false;     // ジャンプ中かどうか
    private float jumpTime = 0f;        // ジャンプの経過時間
    private Vector3 startPosition;      // ジャンプ開始時の位置

    private bool moving_standby = false; //移動待機フラグ

    GameObject left_edge;//左端
    GameObject right_edge;//右端

    GameObject[] Enemy;//敵認識

    private UILoggerWithLimit uiLogger;//死亡ログ

    private Animator animator;//アニメーション

    AudioSource effect_sound;//効果音
    public AudioClip soundEffect;//効果音

    public GameObject attack_effect;//攻撃エフェクト

    public GameObject attack_effect_b;

    GameObject attack_effect_i;

    GameObject attack_effect_b_i;

    const int line_max = 3;

    private int random_value = 0;

    GameObject canvas;
    public GameObject comment;
    GameObject maskObject;

    GameObject sp_storm;
    GameObject sp_guard;
    GameObject sp_step;

    Special_Storm special_storm;
    Special_Guard special_guard;
    Special_Step special_step;

    GameObject r_button;
    GameObject l_button;

    Button right_Button; // 右のボタン
    Button left_Button; // 左のボタン

    [System.Serializable]
    public class NameList
    {
        public List<string> names;
    }

    public void Initialize(string c_type, string c_name, float c_hp, int c_strengh, float c_speed, float c_attack_frequency,float c_contact_range, float c_attack_scope, int c_reaction_rate_max, int c_reaction_rate_min, List<string> c_comments)
    {
        type = c_type;
        name_of_death = c_name;
        hp = c_hp;
        strengh = c_strengh;
        speed = c_speed;
        attack_frequency = c_attack_frequency;
        contact_range = UnityEngine.Random.Range(c_contact_range, c_attack_scope);
        attack_scope = c_attack_scope;
        reaction_rate_max = c_reaction_rate_max;
        reaction_rate_min = c_reaction_rate_min;
        comments = c_comments;

        random_value = UnityEngine.Random.Range(1, 2);

        Comment_spawn(comments[random_value]);
    }

    void Start()
    {
        currentAttackIndex = 0;
        moving_standby = false;
        isJumping = false;
        direction = 1;
        direction = 1;
        knockback_flag = false;
        isPerformingAction = true;
        boss = GameObject.Find("Boss");
        canvas = GameObject.Find("画面上のボタン");
        attack_flag = false;
        this.sr = GetComponent<SpriteRenderer>();
        left_edge = GameObject.Find("左端");
        right_edge = GameObject.Find("右端");

        sp_storm = GameObject.Find("スキル1");
        sp_guard = GameObject.Find("スキル2");
        sp_step = GameObject.Find("スキル4");

        special_storm = sp_storm.GetComponent<Special_Storm>();
        special_guard = sp_guard.GetComponent<Special_Guard>();
        special_step = sp_step.GetComponent<Special_Step>();

        startPosition = transform.position;// ジャンプ開始時の位置を保存

        uiLogger = FindObjectOfType<UILoggerWithLimit>();

        animator = GetComponent<Animator>();

        effect_sound = GetComponent<AudioSource>();

        r_button = GameObject.Find("右ボタン");
        right_Button = r_button.GetComponent<Button>();
        l_button = GameObject.Find("左ボタン");
        left_Button = l_button.GetComponent<Button>();

        right_Button.onClick.AddListener(OnButtonClick_right);
        left_Button.onClick.AddListener(OnButtonClick_left);
    }

    void Update()
    {
        Moving();

        CheckForAttacks();

        Vector2 position = transform.position;

        // x座標とy座標の範囲をClampで制限
        position.x = Mathf.Clamp(position.x, Mathf.Min(left_edge.transform.position.x, right_edge.transform.position.x), Mathf.Max(left_edge.transform.position.x, right_edge.transform.position.x));

        // 位置を更新
        transform.position = position;

        //★
        //ノックバック動作
        if (knockback_flag == true)
        {
            Destroy(AO);
            isPerformingAction = false;

            knockback();
        }
    }
    private void OnDestroy()
    {
        Debug.unityLogger.logEnabled = true;
        
        Destroy(AO);

        Destroy(attack_effect_i);
        Destroy(attack_effect_b_i);
        
    }

    private void Moving()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnButtonClick_left();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnButtonClick_right();
        }
        

        // 方向に基づいて移動
        if (isPerformingAction && !attack_flag) // この条件が移動制御のためのスイッチ
        {
            if (special_step.skill_flag == true)
            {
                transform.Translate(Vector2.right * direction * speed * 2 * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
            }
        }

        GameObject target = FindNearestAllyInAttackRange();

        if (boss)
        {
            if (transform.position.x + contact_range < boss.transform.position.x)
            {
                if (target != null)
                {
                    isPerformingAction = false;
                }

                if (target == null)
                {
                    isPerformingAction = true;
                }
            }
            else
            {
                isPerformingAction = false;
            }
        }

        

        //止まっていたら攻撃する
        if (!isPerformingAction && !attack_flag)
        {
            if (attackPattern != null && attackPattern.attacksequence.Count > 0)
            {
                StartCoroutine(ExecuteAttacksequence(target));
            }
        }

        if (isJumping == true)
        {
            Jumping();
        }

    }

    private void CheckForAttacks()
    {


        if (special_storm.skill_flag == false)
        {
            if (this.hp <= 0)
            {
                uiLogger.AddLog(type + "の" + name_of_death + "が死亡");

                Comment_spawn(comments[8]);

                Destroy(this.gameObject);
                Destroy(attack_effect_i);
                Destroy(attack_effect_b_i);
            }
        }

        // 敵の周囲に攻撃オブジェクトが存在するかチェック
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 2);
        foreach (Collider2D attackCollider in hitColliders)
        {
            // 攻撃オブジェクトかどうか確認
            if (attackCollider.CompareTag("B_S_Attack")　|| attackCollider.CompareTag("B_Attack"))
            {
                GameObject attackObject = attackCollider.gameObject;

                // まだこの攻撃オブジェクトにヒットしていない場合のみ処理を実行
                if (!hitAttacks.Contains(attackObject))
                {
                    Attack_Object attack_object_e = attackObject.GetComponent<Attack_Object>();

                    int damage = attack_object_e.attack_point;

                    if (special_guard.skill_flag == false)
                    {
                        hp = hp - damage;
                    }

                    if (hp <= (hp_max * 0.5) && hp > (hp_max * 0.3))
                    {
                        random_value = UnityEngine.Random.Range(3, 4);

                        Comment_spawn(comments[random_value]);
                    }else if (hp <= (hp_max * 0.3))
                    {
                        random_value = UnityEngine.Random.Range(5, 6);

                        Comment_spawn(comments[random_value]);
                    }

                    // この攻撃オブジェクトを記録して、再度当たり判定が起きないようにする
                    hitAttacks.Add(attackObject);

                    //★
                    knockback_flag = true;
                    
                }

                if (attackCollider.CompareTag("B_S_Attack"))
                {
                    float r_w = UnityEngine.Random.Range(-1.0f, 1.0f);

                    float r_h = UnityEngine.Random.Range(-1.0f, 1.0f);

                    attack_effect_b_i = Instantiate(attack_effect_b, transform.position + new Vector3(r_w, r_h, 0), Quaternion.identity);

                    StartCoroutine(DeleteAfterAnimation(attack_effect_b_i));
                }
            }
        }

        
    }

    //攻撃シーケンス
    IEnumerator ExecuteAttacksequence(GameObject target)
    {
        

        // 現在の行動を取得
        string currentAction = attackPattern.attacksequence[currentAttackIndex];

        int random_value = UnityEngine.Random.Range(0, 2);
        int random_value_reaction;

        attack_flag = true; // 攻撃後にフラグをオンにする
        movement_disabled_flag = true;
        isPerformingAction = false;

        if (special_storm.skill_flag == true)
        {
            currentAction = "Attack";
            Debug.Log(name_of_death + ":" + currentAction);
        }

        switch (currentAction)
        {
            case "Rest":
                Destroy(AO);
                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                yield return new WaitForSeconds(attack_frequency / 60);

                break;

            case "Attack":

                animator.SetTrigger("Attack");

                effect_sound.PlayOneShot(soundEffect);
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

                
                if (random_value == 0)
                {
                    if (!target)
                    {
                        target = boss;
                    }

                    AttackNearestAllyInRange(target);

                    float r_w = UnityEngine.Random.Range(-1.0f, 1.0f);

                    float r_h = UnityEngine.Random.Range(-1.0f, 1.0f);

                    attack_effect_i = Instantiate(attack_effect, transform.position + new Vector3(contact_range + r_w, r_h, 0), Quaternion.identity);

                    StartCoroutine(DeleteAfterAnimation(attack_effect_i));
                }
                else if (random_value == 1)
                {
                    Attack_Object AO_I;//攻撃オブジェクト変換格納用インスタンス

                    float r_w = UnityEngine.Random.Range(-1.0f, 1.0f);

                    float r_h = UnityEngine.Random.Range(-1.0f, 1.0f);

                    AO = Instantiate(attack_Object, transform.position + new Vector3(attack_scope / 2, 0, 0), Quaternion.identity);

                    AO_I = AO.GetComponent<Attack_Object>();

                    AO_I.Initialize(strengh, features_point);

                    attack_effect_i = Instantiate(attack_effect, transform.position + new Vector3(contact_range + r_w, r_h, 0), Quaternion.identity);

                    StartCoroutine(DeleteAfterAnimation(attack_effect_i));
                }

                animator.SetTrigger("Idle");

                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                //yield return new WaitForSeconds(1.0f);

                movement_disabled_flag = false;

                random_value_reaction = UnityEngine.Random.Range(reaction_rate_min, reaction_rate_max);

                if (moving_standby == true)
                {
                    if (movement_r_flag == true && movement_l_flag == false)
                    {
                        StartCoroutine(ChangeDirectionWithDelay_right(random_value_reaction));
                    }
                    else if (movement_l_flag == true && movement_r_flag == false)
                    {
                        StartCoroutine(ChangeDirectionWithDelay_left(random_value_reaction));
                    }
                }

                break;

            default:
                Debug.Log("Unknown action: ");
                break;
        }

        // 次の行動に進む
        Destroy(AO);
        currentAttackIndex = (currentAttackIndex + 1) % attackPattern.attacksequence.Count;
        attack_flag = false; // 攻撃後にフラグを解除して再度攻撃可能に
        movement_disabled_flag = false;
    }

    IEnumerator ChangeDirectionWithDelay_left(int reaction_rate)
    {
        if (special_step.skill_flag == true)
        {
            yield return new WaitForSeconds(reaction_rate * 2);
        }
        else
        {
            yield return new WaitForSeconds(reaction_rate);
        }

        jumpTime = 0f;

        sr.flipX = true;
        direction = -1;//左

        // ジャンプ開始
        isJumping = true;

        if (!isPerformingAction)
        {
            Destroy(AO);
            moving_standby = false;
            attack_flag = false;
            isPerformingAction = true; // 移動を再開
        }
    }
    IEnumerator ChangeDirectionWithDelay_right(int reaction_rate)
    {
        if (special_step.skill_flag == true)
        {
            yield return new WaitForSeconds(reaction_rate * 2);
        }
        else
        {
            yield return new WaitForSeconds(reaction_rate);
        }

        jumpTime = 0f;

        sr.flipX = false;
        direction = 1;//右

        // ジャンプ開始
        isJumping = true;

        if (!isPerformingAction)
        {
            Destroy(AO);
            moving_standby = false;
            attack_flag = false;
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
            // ジャンプ終了時、Y軸位置を元に戻す
            transform.position = new Vector2(transform.position.x, startPosition.y); // Y軸位置をリセット
            knockback_flag = false; // のけぞりフラグを解除
            isPerformingAction = true;
            jumpTime = 0f;
        }
    }

    //１番近い敵を見つける
    GameObject FindNearestAllyInAttackRange()
    {
        GameObject nearestAlly = null;
        float shortestDistance = Mathf.Infinity;

        Enemy = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject ally in Enemy)
        {
            // ボスと味方ユニット間の距離を計算
            float distance = Vector2.Distance(transform.position, ally.transform.position);

            // ユニットが攻撃範囲内にあるか確認
            if (distance <= contact_range && distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestAlly = ally;
            }
        }

        return nearestAlly;
    }

    //通常単体攻撃
    void AttackNearestAllyInRange(GameObject target)
    {
        // ターゲットに攻撃する処理
        IAttackable attackable = target.GetComponent<IAttackable>();

        if (attackable != null)
        { 
            attackable.ApplyDamage(strengh);
        
        }

    }

    private IEnumerator DeleteAfterAnimation(GameObject instance)
    {
        Animator animator_e = instance.GetComponent<Animator>();

        // アニメーションの長さを取得
        float animationLength = animator_e.GetCurrentAnimatorStateInfo(0).length;

        while (true)
        {
            if (this == null)
            {
                Destroy(instance);
            }
            // アニメーションが終了するまで待つ
            yield return new WaitForSeconds(animationLength);
            // オブジェクトを削除
            Destroy(instance);
        }
    }

    public void Comment_spawn(string comment_text)
    {
        random_value = UnityEngine.Random.Range(0, line_max);

        float line = random_value * 30;
        GameObject comment_i;

        canvas = GameObject.Find("画面上のボタン");

        maskObject = GameObject.Find("マスク");

        comment_i = Instantiate(comment);

        comment_i.transform.SetParent(maskObject.transform, false);

        TMP_Text c_Text = comment_i.GetComponentInChildren<TMP_Text>();

        c_Text.text = comment_text;

        RectTransform rectTransform = comment_i.GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector2(Screen.width / 2, line);
    }

    void OnButtonClick_right()
    {
        if (!movement_disabled_flag)
        {
            // 現在の時間を取得
            float currentTime = Time.time;

            float total = currentTime - lastPressTime_r;

            int random_value_reaction = UnityEngine.Random.Range(reaction_rate_min, reaction_rate_max);

            // 前回の押下からの経過時間が設定した連打時間内であれば
            if (total >= doublePressTime || direction == -1)
            {
                StartCoroutine(ChangeDirectionWithDelay_right(random_value_reaction));
            }

            lastPressTime_r = currentTime;
        }
        else
        {
            movement_r_flag = true;
            movement_l_flag = false;

            //待機フラグON
            moving_standby = true;
        }
    }

    void OnButtonClick_left()
    {
        if (!movement_disabled_flag)
        {
            // 現在の時間を取得
            float currentTime = Time.time;

            float total = currentTime - lastPressTime_l;

            int random_value_reaction = UnityEngine.Random.Range(reaction_rate_min, reaction_rate_max);

            // 前回の押下からの経過時間が設定した連打時間内であれば
            if (total >= doublePressTime || direction == 1)
            {
                StartCoroutine(ChangeDirectionWithDelay_left(random_value_reaction));
            }

            lastPressTime_l = currentTime;
        }
        else
        {
            movement_r_flag = false;
            movement_l_flag = true;

            //待機フラグON
            moving_standby = true;
        }
    }
}
