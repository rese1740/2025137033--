using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{

    [Header("이동")]
    public int speed = 5;
    public float jumpForce = 100f;
    public float bulletSpeed = 10f;
    public bool isJumping = false;

    [Header("공격")]
    public GameObject bullet_;
    public Transform gunPos;
    private bool isFacingRight = true;

    [Header("체력")]
    public float playerHealth = 10.0f;
    public Slider playerHealthSlider;
    private bool Invincible = false;


    [Header("기타")]
    public Animator myAnimator;
    private Rigidbody2D rb;
    private GameObject currentpotal, currentPotal1, currentPotal2;


    private void Start()
    {
        myAnimator.SetBool("move", false);
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float direction = Input.GetAxis("Horizontal");

        // 방향에 따른 스케일 수정 및 애니메이션 상태 변경
        if (direction != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction) * 10f, 10f, 1); // 방향에 맞춰 스케일을 수정
            myAnimator.SetBool("move", true);  // 이동 중인 상태
        }
        else
        {
            myAnimator.SetBool("move", false);  // 정지 상태
        }
        if (transform.localScale.x > 0)
        {
            isFacingRight = true;
        }
        else if (transform.localScale.x < 0)
        {
            isFacingRight = false;
        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJumping)
            {
                Jump();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentpotal != null)
            {
                SceneManager.LoadScene("DoorScene");
            }

            if (currentPotal1 != null)
            {
                SceneManager.LoadScene("DoorScene1");
            }
            if (currentPotal2 != null)
            {
                SceneManager.LoadScene("MainScene");
            }
        }

        if (Input.GetMouseButtonDown(0)) // 마우스 좌클릭이 눌리면
        {
            myAnimator.SetTrigger("attack");

            Vector2 shootDirection = isFacingRight ? Vector2.right : Vector2.left;  // 오른쪽이면 (1, 0), 왼쪽이면 (-1, 0)

            // 총알 생성
            GameObject bullet = Instantiate(bullet_, gunPos.position, Quaternion.identity);

            // 총알의 방향 설정
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(shootDirection);
            }

            // 총알 속도 설정
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = shootDirection * bulletSpeed;
            }
        }

        playerHealthSlider.value = playerHealth;

        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
    }


    #region 닿는 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region 태그
        if (collision.CompareTag("Potal"))
        {
            currentpotal = collision.gameObject;
        }
        else if (collision.CompareTag("Potal1"))
        {
            currentPotal1 = collision.gameObject;
        }
        else if (collision.CompareTag("Potal2"))
        {
            currentPotal2 = collision.gameObject;
        }
        else if (collision.CompareTag("SpeedItem"))
        {
            speed += 10;
            Invoke("SpeedUp", 5f);
        }
        else if (collision.CompareTag("Invincible"))
        {
            Invincible = true;
            Invoke("InvincibleUp", 5f);
        }
        #endregion

        switch (collision.gameObject.tag)
        {
            case "SpeedItem":
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Potal"))
        {
            if (collision.gameObject == currentpotal)
            {
                currentpotal = null;
            }
        }
        else if (collision.CompareTag("Potal1"))
        {
            if (collision.gameObject == currentpotal)
            {
                currentPotal1 = null;
            }
        }
        else if (collision.CompareTag("Potal2"))
        {
            if (collision.gameObject == currentpotal)
            {
                currentPotal2 = null;
            }
        }
    }
    #endregion

    void Jump()
    {
        isJumping = true;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    public void TakeDamage(float damage)
    {
        myAnimator.SetTrigger("damage");
        if (!Invincible)
        {
            playerHealth -= damage;
        }
    }

    public void SpeedUp()
    {
        speed -= 10;
    }
    void InvincibleUp()
    {
        Invincible = false;
    }
}
