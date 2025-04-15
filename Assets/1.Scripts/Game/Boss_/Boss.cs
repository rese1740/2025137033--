using System.Collections;
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

    [Header("보스 패턴1")]
    public float attackCooldown = 3.0f; // 공격 쿨타임 (초)
    public GameObject warningEffectPrefab;
    private Coroutine attack1Routine;
    private int attack1Count = 0;
    public Transform effectSpawnPoint;

    [Header("보스 공격 프리팹")]
    public GameObject slashPrefab;
    public Transform firePoint; 
    public GameObject bigSlashEffect;

    [Header("기타")]
    public float bulletDamage = 5.0f;
    public Transform player;
    public LayerMask playerLayer;  

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        attack1Routine = StartCoroutine(BossAttack1Loop());
    }

    private void Update()
    {
        if (!isAlive) return;

        hpSlider.value = bossHealth;

        if (bossHealth <= 50)
        {
            hpImg.color = Color.red;
        }

        if (bossHealth <= 14)
        {
            SceneManager.LoadScene("Ending_Scene");
            isAlive = false;
            BossDeath();
        }

        // 보스가 플레이어 바라보게
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
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



    private IEnumerator BossAttack1Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackCooldown);

            anim.SetTrigger("attack");

            Vector2 directionToPlayer = (player.position - transform.position).normalized;

            GameObject slash = Instantiate(slashPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D slashRb = slash.GetComponent<Rigidbody2D>();

            if (slashRb != null)
            {
                float slashSpeed = 10f;
                slashRb.velocity = directionToPlayer * slashSpeed;
            }

            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            slash.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            attack1Count++;

            if (attack1Count >= 3)
            {
                attack1Count = 0;
                yield return StartCoroutine(BossAttack2());
            }
        }
    }

    private IEnumerator BossAttack2()
    {
        anim.SetTrigger("attack2");

        GameObject warning = Instantiate(warningEffectPrefab, effectSpawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(0.8f);

        Instantiate(bigSlashEffect, effectSpawnPoint.position, Quaternion.identity);
        Destroy(warning, 0.5f);

        yield return new WaitForSeconds(1.0f); // 약간의 여유 시간 (이펙트 끝날 때까지)
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

}
