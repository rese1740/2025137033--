using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [Header("보스 UI")]
    public Slider hpSlider;
    public Image hpImg;

    [Header("보스 스탯")]
    public float bossHealth = 100.0f;
    public float bossSpeed = 1.5f;  // 이동 속도
    public float bossDamage = 5.0f;
    public float attackRange = 2.0f;  // 공격 범위
    public bool isAlive = true;
    private float attackCooldown = 3.0f; // 공격 쿨타임 (초)
    private float lastAttackTime = -Mathf.Infinity; // 마지막 공격 시간
    private int attack1Count = 0;


    [Header("보스 공격 프리팹")]
    public GameObject slashPrefab;
    public Transform firePoint; // Slash가 생성될 위치 (보스 손 앞 등)

    [Header("기타")]
    public float bulletDamage = 5.0f;
    public Transform player;
    public LayerMask playerLayer;  // 플레이어 탐색을 위한 레이어


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
                rb.velocity = Vector2.zero;  // 공격 중일 때는 멈춤
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

        // x축 방향만 이동, y축은 현재 높이 유지
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
        Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;

        rb.velocity = new Vector2(moveDirection.x * bossSpeed, rb.velocity.y);

        // 애니메이션 방향 전환 (좌우 이동 시)
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
            return; // 쿨타임이 끝나지 않았다면 공격하지 않음

        lastAttackTime = Time.time; // 마지막 공격 시간 업데이트

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

        // 공격 횟수 증가 및 체크
        attack1Count++;

        if (attack1Count >= 4)
        {
            attack1Count = 0; // 초기화
            BossAttack2();    // 순간이동 공격 발동
        }
    }


    public void BossAttack2()
    {
        if (slashPrefab == null || firePoint == null) return;

        anim.SetTrigger("attack2");

        // 보스가 바라보는 방향 확인
        float direction = transform.localScale.x > 0 ? 1f : -1f;

        // 프리팹 생성
        GameObject slash = Instantiate(slashPrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D slashRb = slash.GetComponent<Rigidbody2D>();
        if (slashRb != null)
        {
            float slashSpeed = 10f;
            slashRb.velocity = new Vector2(direction * slashSpeed, 0f);
        }

        // 슬래시 방향도 회전
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
        Gizmos.DrawWireSphere(transform.position, 10f); // 플레이어 탐색 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange); // 공격 범위
    }
}
