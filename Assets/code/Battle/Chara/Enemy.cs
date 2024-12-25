using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour, IAttackable
{
    private HashSet<GameObject> hitAttacks = new HashSet<GameObject>();//�U���d���`�F�b�N
    public int hp { get; set; }//�q�b�g�|�C���g
    private int strengh;//�U����
    private bool isPerformingAction = true;//�ړ��t���O
    private int direction = 1;//���]����
    public bool knockback_flag = false;//�̂�����t���O
    private float speed;//�f����
    private float attack_frequency;//�U���p�x
    private int attack_scope;//�U���͈�
    private bool attack_flag;//�U���t���O

    public UnitAttackPattern attackPattern;//�p�^�[���i�[�ϐ�
    private int currentAttackIndex = 0;//�p�^�[���Ǘ��ϐ�

    GameObject left_edge;
    GameObject right_edge;

    GameObject[] Villager;

    GameObject AO;//�U���p�I�u�W�F�N�g�C���X�^���X�p�ϐ�
    public GameObject Attack_Object;//�U���p�I�u�W�F�N�g�i�[�p�ϐ�

    public float jumpHeight = 0.8f;       // �W�����v�̍���
    public float jumpDuration = 0.5f;     // �W�����v�ɂ����鎞��
    private float jumpTime = 0f;        // �W�����v�̌o�ߎ���
    private Vector3 startPosition;      // �W�����v�J�n���̈ʒu

    GameObject sp_guard;
    Special_Guard special_guard;

    public void Initialize(int c_strengh,int c_hp, float c_speed, int c_attack_scope, float c_attack_frequency)
    {
        strengh = c_strengh;
        hp = c_hp;
        speed = c_speed;
        attack_scope = c_attack_scope;
        attack_frequency = c_attack_frequency;
    }

    // Start is called before the first frame update
    void Start()
    {
        knockback_flag = false;
        attack_flag = false;
        isPerformingAction = true;
        direction = 1;
        left_edge = GameObject.Find("���[");
        right_edge = GameObject.Find("�E�[");
        sp_guard = GameObject.Find("�X�L��2");
        special_guard = sp_guard.GetComponent<Special_Guard>();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();

        CheckForAttacks();

        Vector2 position = transform.position;

        // x���W��y���W�͈̔͂�Clamp�Ő���
        position.x = Mathf.Clamp(position.x, Mathf.Min(left_edge.transform.position.x, right_edge.transform.position.x), Mathf.Max(left_edge.transform.position.x, right_edge.transform.position.x));

        // �ʒu���X�V
        transform.position = position;

        //�m�b�N�o�b�N����
        if (knockback_flag == true)
        {
            Destroy(AO);
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
        // �����Ɋ�Â��Ĉړ�
        if (isPerformingAction) // ���̏������ړ�����̂��߂̃X�C�b�`
        {
            transform.Translate(Vector2.left * direction * speed * Time.deltaTime);
        }


        
        if (FindNearestAllyInAttackRange_s() != null)
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
                StartCoroutine(ExecuteAttacksequence());
            }
        }
    }

    //�P�ԋ߂��G��������
    GameObject FindNearestAllyInAttackRange_s()
    {
        GameObject nearestAlly = null;
        float shortestDistance = Mathf.Infinity;

        int random_value = UnityEngine.Random.Range(1, attack_scope);

        Villager = GameObject.FindGameObjectsWithTag("Villager");

        foreach (GameObject ally in Villager)
        {
            // �{�X�Ɩ������j�b�g�Ԃ̋������v�Z
            float distance = Vector2.Distance(transform.position, ally.transform.position);

            // ���j�b�g���U���͈͓��ɂ��邩�m�F
            if (distance <= 1 && distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestAlly = ally;
            }
        }
        return nearestAlly;
    }

    //�P�ԋ߂��G��������
    GameObject FindNearestAllyInAttackRange()
    {
        GameObject nearestAlly = null;
        float shortestDistance = Mathf.Infinity;

        Villager = GameObject.FindGameObjectsWithTag("Villager");

        foreach (GameObject ally in Villager)
        {
            // �{�X�Ɩ������j�b�g�Ԃ̋������v�Z
            float distance = Vector2.Distance(transform.position, ally.transform.position);

            // ���j�b�g���U���͈͓��ɂ��邩�m�F
            if (distance <= attack_scope && distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestAlly = ally;
            }
        }
        return nearestAlly;
    }

    IEnumerator ExecuteAttacksequence()
    {
        // ���݂̍s�����擾
        string currentAction = attackPattern.attacksequence[currentAttackIndex];

        int random_value = UnityEngine.Random.Range(0, 2);

        attack_flag = true; // �U����Ƀt���O���I���ɂ���

        switch (currentAction)
        {
            case "Rest":
                Destroy(AO);
                // �s�����ƂɈقȂ鎞�Ԃ�҂i���ɍU���p�x���g�p���đҋ@���Ԃ�ݒ�j
                yield return new WaitForSeconds(10.0f / attack_frequency);

                break;

            case "Attack":

                if (random_value == 0)
                {
                    AttackNearestAllyInRange();
                }
                else if (random_value == 1)
                {
                    AO = Instantiate(Attack_Object, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
                }

                // �s�����ƂɈقȂ鎞�Ԃ�҂i���ɍU���p�x���g�p���đҋ@���Ԃ�ݒ�j
                yield return new WaitForSeconds(1.0f);

                break;

            default:
                //Debug.Log("Unknown action: ");
                break;
        }

        // ���̍s���ɐi��
        Destroy(AO);
        currentAttackIndex = (currentAttackIndex + 1) % attackPattern.attacksequence.Count;
        attack_flag = false; // �U����Ƀt���O���������čēx�U���\��
    }

    //�ʏ�P�̍U��
    void AttackNearestAllyInRange()
    {
        GameObject target = FindNearestAllyInAttackRange();
        // �^�[�Q�b�g�ɍU�����鏈��

        if (target != null)
        {
            Unit unit = target.GetComponent<Unit>();

            if (special_guard.skill_flag == false)
            {
                unit.hp = unit.hp - strengh;
            }

            if (unit.hp <= 50 && unit.hp > 30)
            {
                int random_value = UnityEngine.Random.Range(2, 3);

                unit.Comment_spawn(unit.comments[random_value],1);
            }
            else if (unit.hp <= 30)
            {
                int random_value = UnityEngine.Random.Range(4, 5);

                unit.Comment_spawn(unit.comments[random_value],2);
            }

            unit.knockback_flag = true;
        }
    }

    public void ApplyDamage(int damage)
    {
        hp -= damage;
        knockback_flag = true;
    }

    private void CheckForAttacks()
    {
        // �G�̎��͂ɍU���I�u�W�F�N�g�����݂��邩�`�F�b�N
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 2);
        foreach (Collider2D attackCollider in hitColliders)
        {
            // �U���I�u�W�F�N�g���ǂ����m�F
            if (attackCollider.CompareTag("Attack"))
            {
                GameObject hitObject = attackCollider.gameObject;
                Attack_Object attackObject = hitObject.GetComponent<Attack_Object>();

                // �܂����̍U���I�u�W�F�N�g�Ƀq�b�g���Ă��Ȃ��ꍇ�̂ݏ��������s
                if (!hitAttacks.Contains(hitObject))
                {
                    this.hp = this.hp - attackObject.attack_point;

                    // ���̍U���I�u�W�F�N�g���L�^���āA�ēx�����蔻�肪�N���Ȃ��悤�ɂ���
                    hitAttacks.Add(hitObject);

                    //��
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

        // �O�p�֐����g����Y�������̈ړ����v�Z
        float progress = jumpTime / jumpDuration; // �W�����v�̐i��
        float yOffset = Mathf.Sin(Mathf.PI * progress) * jumpHeight; // sin�J�[�u��Y���̈ړ��ʂ�����

        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // X���̂̂�����ړ���Y���̃W�����v�𓯎��ɓK�p
        transform.position = new Vector2(transform.position.x, startPosition.y + yOffset);

        // �W�����v���I���������ǂ������m�F
        if (progress >= 1f)
        {

            // �W�����v�I�����AY���ʒu�����ɖ߂�
            transform.position = new Vector2(transform.position.x, startPosition.y); // Y���ʒu�����Z�b�g
            knockback_flag = false; // �̂�����t���O������
            isPerformingAction = true;
            jumpTime = 0f;
        }
    }
}
