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

    [Header("보스 공격 프리팹")]
    public GameObject slashPrefab;
    public Transform firePoint; // Slash가 생성될 위치 (보스 손 앞 등)
    public GameObject bigSlashEffect;

    [Header("기타")]
    public float bulletDamage = 5.0f;
    public Transform player;
    public LayerMask playerLayer;  // 플레이어 탐색을 위한 레이어
    private Vector2 direction_;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(BossAttack1());
    }

    private void Update()
    {
        if (!isAlive) return;
       
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

      

        // 보스가 플레이어 바라보게
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            direction_ = new Vector2(-1, 0);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            direction_ = new Vector2(1, 0);
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



    IEnumerator BossAttack1()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackCooldown);

            anim.SetTrigger("attack");
           
            GameObject slash = Instantiate(slashPrefab, firePoint.position, Quaternion.identity);

            Rigidbody2D slashRb = slash.GetComponent<Rigidbody2D>();
            if (slashRb != null)
            {
                float slashSpeed = 10f;
                slashRb.velocity = direction_ * slashSpeed;
            }

            // 슬래시 스프라이트 방향 조정
            Vector3 slashScale = slash.transform.localScale;
            slashScale.x = direction_.x > 0 ? Mathf.Abs(slashScale.x) : -Mathf.Abs(slashScale.x);
            slash.transform.localScale = slashScale;
        }
    }


   
    public void BossAttack2()
    {
        anim.SetTrigger("attack2");
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
