using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.GraphicsBuffer;

public class Boss : MonoBehaviour, IAttackable
{
    private int hp_max;
    public int hp { get; set; }//HP
    private int strengh;//攻撃力
    private float attack_frequency;//攻撃頻度
    private float attack_scope;//攻撃範囲
    //private List<string> features_point;//ダメージ増減倍率
    private int Level_max;
    public int Level;//レベル
    private float experience;//経験値
    private float experience_reference;//経験値

    public BossAttackPattern attackPattern;//パターン格納変数
    private int currentAttackIndex = 0;//パターン管理変数
    private int currentMeteorIndex = 0;//パターン管理変数
    private int currentBeamIndex = 0;//パターン管理変数
    private int currentBulletIndex = 0;//パターン管理変数

    private List<string> features_point;//ダメージ増減倍率

    public GameObject meteorite_place_1;
    public GameObject meteorite_place_2;

    private bool attack_flag;
    private bool bullet_flag;
    private bool beam_flag;
    private bool meteor_flag;
    private HashSet<GameObject> hitAttacks = new HashSet<GameObject>();
    GameObject AO;//攻撃用オブジェクトインスタンス用変数
    public GameObject Meteor;//攻撃用オブジェクト格納用変数
    private Meteorite meteorite;

    GameObject AO_b;//攻撃用オブジェクトインスタンス用変数

    public GameObject Bullet;
    private Bullet bullet;

    public GameObject Beam;
    Attack_Object beam;

    GameObject[] Villager;

    private int attack_count = 0;//攻撃カウント用変数
    private int previous_hp;

    public GameObject Hitcount;
    public GameObject ND;
    public NumberDisplay numberDisplay;

    public Image boss_life_bar;   // タイムゲージのImage

    private Animator animator;

    public GameObject Level_Up;

    GameObject attack_effect_i;//インスタンス用

    public GameObject attack_effect;//攻撃エフェクト

    GameObject sp_guard;
    Special_Guard special_guard;
    public int Hp
    {
        get { return this.hp; }
        set
        {
            this.hp = value;

            attack_count++;

            Hitcount.SetActive(true);
            ND.SetActive(true);

            numberDisplay.DisplayNumber(attack_count);

            // 値が変更された際の処理
            //Debug.Log(attack_count);
        }
    }

    void Start()
    {
        //これらは仮のステータス後でコンストラクタで設定するのでそれを実装したら消す
        hp = 65536;
        hp_max = hp;
        strengh = 5;
        attack_frequency = 2;
        attack_scope = 5;
        Level = 10;
        Level_max = 10;
        experience = 0;
        experience_reference = 3;

        boss_life_bar.fillAmount = 1f;

        attack_count = 0;
        previous_hp = hp;

        //features_point = new List<string> { "大型BOSSに強い", "中型" };

        animator = GetComponent<Animator>();
        sp_guard = GameObject.Find("スキル2");
        special_guard = sp_guard.GetComponent<Special_Guard>();
    }


    void Update()
    {
        double now_bar = Mathf.Clamp(this.hp, 0, hp_max);

        boss_life_bar.fillAmount = (float)(now_bar / hp_max);

        GameObject target = FindNearestAllyInAttackRange();

        if (target != null && !attack_flag)
        {
            Hitcount.SetActive(false);
            ND.SetActive(false);
            attack_count = 0;
            StartCoroutine(ExecuteAttacksequence());
        }

        if (Level >= 4)
        {
            if (target != null && !meteor_flag)
            {
                Hitcount.SetActive(false);
                ND.SetActive(false);
                attack_count = 0;
                StartCoroutine(ExecuteMeteorAttacksequence());
            }
        }

        if (Level >= 8)
        {
            if (target != null && !bullet_flag)
            {
                Hitcount.SetActive(false);
                ND.SetActive(false);
                attack_count = 0;
                StartCoroutine(ExecuteBulletAttacksequence());
            }
        }

        if (Level >= 10)
        {
            if (target != null && !beam_flag)
            {
                Hitcount.SetActive(false);
                ND.SetActive(false);
                attack_count = 0;
                StartCoroutine(ExecuteBeamAttacksequence());
            }
        }

        CheckForAttacks();
    }

    private void OnDestroy()
    {
        Destroy(AO_b);
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
                    Attack_Object attack_object = attack_effect.GetComponent<Attack_Object>();

                    int damage = attack_object.attack_point;

                    if (attack_object != null)
                    {
                        Hp -= attack_object.attack_point;
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

    IEnumerator ExecuteBulletAttacksequence()
    {
        bullet_flag = true;

        Vector3 targetScale = new Vector3(2.0f, 2.0f, 2.0f);

        string currentAction = attackPattern.b_attacksequence[currentBulletIndex];

        switch (currentAction)
        {
            case "Rest":

                if (Level >= 9)
                {
                    yield return new WaitForSeconds(9.0f);
                }
                else
                {
                    // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                    yield return new WaitForSeconds(15.0f);
                }

                break;

            case "Attack":

                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

                AO = Instantiate(Bullet, transform.position + new Vector3(0, 0, 0), Quaternion.identity);

                AO.transform.localScale = targetScale;

                bullet = AO.GetComponent<Bullet>();

                bullet.Initialize(strengh, features_point);

                animator.SetTrigger("Idle");

                break;
        }

        currentBulletIndex = (currentBulletIndex + 1) % attackPattern.b_attacksequence.Count;

        bullet_flag = false;
    }

    IEnumerator ExecuteBeamAttacksequence()
    {
        beam_flag = true;

        string currentAction = attackPattern.b_attacksequence[currentBeamIndex];

        switch (currentAction)
        {
            case "Rest":
                
                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                yield return new WaitForSeconds(15.0f);

                break;

            case "Attack":

                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

                AO_b = Instantiate(Beam, transform.position + new Vector3(-7, 0, 0), Quaternion.identity);

                beam = AO_b.GetComponent<Attack_Object>();

                beam.Initialize(strengh, features_point);

                animator.SetTrigger("Idle");

                yield return new WaitForSeconds(1.0f);

                Destroy(AO_b);

                break;
        }
        
        currentBeamIndex = (currentBeamIndex + 1) % attackPattern.b_attacksequence.Count;

        beam_flag = false;
    }

    IEnumerator ExecuteMeteorAttacksequence()
    {
        meteor_flag = true;

        Vector3 targetScale = new Vector3(2.0f, 2.0f, 2.0f);

        string currentAction = attackPattern.b_attacksequence[currentMeteorIndex];

        switch (currentAction)
        {
            case "Rest":
                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）

                if (Level >= 9)
                {
                    yield return new WaitForSeconds(3.0f);
                }
                else
                {
                    yield return new WaitForSeconds(5.0f);
                }

                break;

            case "Attack":
                
                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

                AO = Instantiate(Meteor, transform.position + new Vector3(0, 7, 0), Quaternion.identity);

                if (Level >= 7)
                {
                    AO.transform.localScale = targetScale;
                }

                meteorite = AO.GetComponent<Meteorite>();

                meteorite.Initialize(strengh, features_point, meteorite_place_1.transform);

                animator.SetTrigger("Idle");

                if (Level >= 6)
                {
                    animator.SetTrigger("Attack");
                    yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

                    AO = Instantiate(Meteor, transform.position + new Vector3(0, 7, 0), Quaternion.identity);

                    if (Level >= 7)
                    {
                        AO.transform.localScale = targetScale;
                    }

                    meteorite = AO.GetComponent<Meteorite>();

                    meteorite.Initialize(strengh, features_point, meteorite_place_2.transform);

                    animator.SetTrigger("Idle");
                }

                break;
        }
        currentMeteorIndex = (currentMeteorIndex + 1) % attackPattern.b_attacksequence.Count;

        meteor_flag = false;
    }

    IEnumerator ExecuteAttacksequence()
    {
        attack_flag = true;

        int normal = 1;

        if (Level >= 2)
        {
            normal = 3;
        }

        // 現在の行動を取得
        string currentAction = attackPattern.b_attacksequence[currentAttackIndex];
        switch (currentAction)
        {
            case "Rest":
                // 行動ごとに異なる時間を待つ（仮に攻撃頻度を使用して待機時間を設定）
                yield return new WaitForSeconds(10.0f / attack_frequency);

                break;

            case "Attack":

                for (int j = 0; j < normal; j++)
                {
                    animator.SetTrigger("Attack");
                    yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

                    AttackNearestAllyInRange();

                    animator.SetTrigger("Idle");
                }

                yield return new WaitForSeconds(1.0f);

                

                break;

            default:
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

    

    List<GameObject> SortAlliesByDistance()
    {
        // Villagerを取得
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Villager");

        // リストに変換して距離順にソート
        List<GameObject> sortedAllies = new List<GameObject>(allies);
        sortedAllies.Sort((a, b) =>
        {
            float distanceA = Vector2.Distance(transform.position, a.transform.position);
            float distanceB = Vector2.Distance(transform.position, b.transform.position);
            return distanceA.CompareTo(distanceB); // 距離の昇順でソート
        });

        return sortedAllies;
    }



    public void ApplyDamage(int damage)
    {
        Hp -= damage;
    }

    //ボスの通常攻撃
    void AttackNearestAllyInRange()
    {
        List<GameObject> target_i = SortAlliesByDistance();

        int count_a = 1;

        if (Level >= 3)
        {
            count_a = 2;
        }

        if (Level >= 5)
        {
            count_a = 3;
        }

        if(count_a > target_i.Count)
        {
            count_a = target_i.Count;
        }

        for (int i = 0; i < count_a; i++)
        {
            Unit unit = target_i[i].GetComponent<Unit>();
            
            if (unit.hp <= 0)
            {
                Debug.Log(unit.name_of_death);

                experience++;

                unit = target_i[i++].GetComponent<Unit>();
            }

            if (special_guard.skill_flag == false)
            {
                unit.hp = unit.hp - strengh;
            }

            float r_w = UnityEngine.Random.Range(-1.0f, 1.0f);

            float r_h = UnityEngine.Random.Range(-1.0f, 1.0f);

            attack_effect_i = Instantiate(attack_effect, target_i[i].transform.position + new Vector3(r_w, r_h, 0), Quaternion.identity);

            StartCoroutine(DeleteAfterAnimation(attack_effect_i));

            unit.knockback_flag = true;
        }

        if(experience >= experience_reference && Level < Level_max)
        {
            Level++;
            experience_reference = experience_reference * 1.2f;

            Level_Up.SetActive(true);

            Debug.Log("Level:" + Level);

            experience = 0;
        }
    }

    private IEnumerator DeleteAfterAnimation(GameObject instance)
    {
        Animator animator = instance.GetComponent<Animator>();

        // アニメーションの長さを取得
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        // アニメーションが終了するまで待つ
        yield return new WaitForSeconds(animationLength);

        // オブジェクトを削除
        Destroy(instance);
    }
}
