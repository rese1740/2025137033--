using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private Animator anim;

    [Header("���� UI")]
    public Slider hpSlider;

    //���� ����
    [Header("���� ����")]
    public float bossHealth = 100.0f;
    public float bossSpeed = 1.0f;
    public float bossDamage = 5.0f;
    public float attackRange = 3.0f;
    public bool isAlive = true;

    [Header("��Ÿ")]
    public float bulletDamage = 5.0f;
    public Transform player;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (bossHealth <= 0)
        {
            if (isAlive)
            {
                isAlive = false;
                BossDeath();
            }   
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // �÷��̾ ���� ��ó(3 ����)�� ������ ����
        if (distanceToPlayer <= attackRange)
        {
            BossAttack1();  // ���� ����
        }

        

        hpSlider.value = bossHealth;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            BossDamage();
            bossHealth -= bulletDamage;
            Destroy(collision.gameObject);
        }
    }

    public void BossAttack1()
    {
        anim.SetTrigger("attack");

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Player"));

        foreach (Collider2D playerCollider in hitPlayers)
        {
            // �÷��̾�� �������� �ִ� �κ�
            PlayerStat playerHealth = playerCollider.GetComponent<PlayerStat>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(bossDamage);  // �÷��̾��� ü���� ���ҽ�Ŵ
            }
        }
    }


    public void BossAttack2()
    {
        anim.SetTrigger("attack2");
    }

    public void BossDeath()
    {
        anim.SetTrigger("Death");
    }

    public void BossDamage()
    {
        anim.SetTrigger("damage");
    }



}
