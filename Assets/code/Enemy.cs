using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    private HashSet<GameObject> hitAttacks = new HashSet<GameObject>();//�U���d���`�F�b�N
    public float hp;//�q�b�g�|�C���g
    private float strengh;//�U����
    private bool isPerformingAction = true;//�ړ��t���O
    private int direction = 1;//���]����
    public bool knockback_flag = false;//�̂�����t���O
    private float speed;//�f����
    private float attack_frequency;//�U���p�x
    private float attack_scope = 5f;//�U���͈�
    private bool attack_flag;//�U���t���O

    public UnitAttackPattern attackPattern;//�p�^�[���i�[�ϐ�
    private int currentAttackIndex = 0;//�p�^�[���Ǘ��ϐ�

    GameObject[] Villager;

    public float jumpHeight = 0.8f;       // �W�����v�̍���
    public float jumpDuration = 0.5f;     // �W�����v�ɂ����鎞��
    private float jumpTime = 0f;        // �W�����v�̌o�ߎ���
    private Vector3 startPosition;      // �W�����v�J�n���̈ʒu

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

        //�m�b�N�o�b�N����
        if (knockback_flag == true)
        {
            isPerformingAction = false;

            knockback();
        }
    }

    private void Moving()
    {
        // �����Ɋ�Â��Ĉړ�
        if (isPerformingAction) // ���̏������ړ�����̂��߂̃X�C�b�`
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

    IEnumerator ExecuteAttacksequence(GameObject target)
    {
        // ���݂̍s�����擾
        string currentAction = attackPattern.attacksequence[currentAttackIndex];

        attack_flag = true; // �U����Ƀt���O���I���ɂ���

        switch (currentAction)
        {
            case "Rest":
                // �s�����ƂɈقȂ鎞�Ԃ�҂i���ɍU���p�x���g�p���đҋ@���Ԃ�ݒ�j
                yield return new WaitForSeconds(10.0f / attack_frequency);

                break;

            case "Attack":

                AttackNearestAllyInRange(target);

                // �s�����ƂɈقȂ鎞�Ԃ�҂i���ɍU���p�x���g�p���đҋ@���Ԃ�ݒ�j
                yield return new WaitForSeconds(1.0f);

                break;

            default:
                //Debug.Log("Unknown action: ");
                break;
        }

        // ���̍s���ɐi��
        currentAttackIndex = (currentAttackIndex + 1) % attackPattern.attacksequence.Count;
        attack_flag = false; // �U����Ƀt���O���������čēx�U���\��
    }

    //�ʏ�P�̍U��
    void AttackNearestAllyInRange(GameObject target)
    {
        
        // �^�[�Q�b�g�ɍU�����鏈��

        Unit unit = target.GetComponent<Unit>();

        unit.hp = unit.hp - 1;
        Debug.Log(unit.hp);
        unit.knockback_flag = true;
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
