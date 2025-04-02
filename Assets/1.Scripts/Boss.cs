using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [Header("���� UI")]
    public Slider hpSlider;

    [Header("���� ����")]
    public float bossHealth = 100.0f;
    public float bossSpeed = 1.5f;  // �̵� �ӵ�
    public float bossDamage = 5.0f;
    public float attackRange = 2.0f;  // ���� ����
    public bool isAlive = true;
    private float attackCooldown = 5.0f; // ���� ��Ÿ�� (��)
    private float lastAttackTime = -Mathf.Infinity; // ������ ���� �ð�

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

        if (bossHealth <= 0)
        {
            isAlive = false;
            BossDeath();
        }

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
