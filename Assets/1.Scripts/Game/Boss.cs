using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [Header("���� UI")]
    public Slider hpSlider;
    public Image hpImg;

    [Header("���� ����")]
    public float bossHealth = 100.0f;
    public float bossSpeed = 1.5f;  // �̵� �ӵ�
    public float bossDamage = 5.0f;
    public float attackRange = 2.0f;  // ���� ����
    public bool isAlive = true;
    private float attackCooldown = 3.0f; // ���� ��Ÿ�� (��)
    private float lastAttackTime = -Mathf.Infinity; // ������ ���� �ð�
    private int attack1Count = 0;


    [Header("���� ���� ������")]
    public GameObject slashPrefab;
    public Transform firePoint; // Slash�� ������ ��ġ (���� �� �� ��)

    [Header("��Ÿ")]
    public float bulletDamage = 5.0f;
    public Transform player;
    public LayerMask playerLayer;  // �÷��̾� Ž���� ���� ���̾�


    private Vector2 moveDirection;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isAlive) return;

       

        player = FindClosestPlayer();

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                rb.velocity = Vector2.zero;  // ���� ���� ���� ����
                BossAttack1();
            }
            else
            {
                MoveTowardsPlayer();
            }
        }

        hpSlider.value = bossHealth;

        if(bossHealth <= 50)
        {
            hpImg.color = Color.red;   
        }

        if (bossHealth <= 14)
        {
            SceneManager.LoadScene("Ending_Scene");
            isAlive = false;
            BossDeath();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player == null) return;

        // x�� ���⸸ �̵�, y���� ���� ���� ����
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
        Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;

        rb.velocity = new Vector2(moveDirection.x * bossSpeed, rb.velocity.y);

        // �ִϸ��̼� ���� ��ȯ (�¿� �̵� ��)
        if (moveDirection.x > 0)
            transform.localScale = new Vector3(5, 5, 1);
        else
            transform.localScale = new Vector3(-5, 5, 1);

        anim.SetBool("move", true);
    }


    private Transform FindClosestPlayer()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, 10f, playerLayer);
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D p in players)
        {
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = p.transform;
            }
        }
        return closest;
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
        if (Time.time - lastAttackTime < attackCooldown)
            return; // ��Ÿ���� ������ �ʾҴٸ� �������� ����

        lastAttackTime = Time.time; // ������ ���� �ð� ������Ʈ

        anim.SetTrigger("attack");

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Player"));

        foreach (Collider2D playerCollider in hitPlayers)
        {
            PlayerStat playerHealth = playerCollider.GetComponent<PlayerStat>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(bossDamage);
            }
        }

        // ���� Ƚ�� ���� �� üũ
        attack1Count++;

        if (attack1Count >= 4)
        {
            attack1Count = 0; // �ʱ�ȭ
            BossAttack2();    // �����̵� ���� �ߵ�
        }
    }


    public void BossAttack2()
    {
        if (slashPrefab == null || firePoint == null) return;

        anim.SetTrigger("attack2");

        // ������ �ٶ󺸴� ���� Ȯ��
        float direction = transform.localScale.x > 0 ? 1f : -1f;

        // ������ ����
        GameObject slash = Instantiate(slashPrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D slashRb = slash.GetComponent<Rigidbody2D>();
        if (slashRb != null)
        {
            float slashSpeed = 10f;
            slashRb.velocity = new Vector2(direction * slashSpeed, 0f);
        }

        // ������ ���⵵ ȸ��
        Vector3 slashScale = slash.transform.localScale;
        slashScale.x = direction * Mathf.Abs(slashScale.x);
        slash.transform.localScale = slashScale;
    }


    public void BossDeath()
    {
        anim.SetTrigger("Death");
        rb.velocity = Vector2.zero;
    }

    public void BossDamage()
    {
        anim.SetTrigger("damage");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10f); // �÷��̾� Ž�� ����
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange); // ���� ����
    }
}
