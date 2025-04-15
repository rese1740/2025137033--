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
    private Vector3 startPos;


    [Header("공격")]
    public GameObject bullet_;
    public Transform gunPos;
    private bool isFacingRight = true;
    private float damage_ = 5.0f;
    //스킬 1
    public float skillDamage = 0;
    public Slider skillSlider;


    [Header("체력")]
    public Slider playerHealthSlider;
    private bool Invincible = false;


    [Header("기타")]
    public Animator myAnimator;
    public Image playerImg;
    public Sprite[] pImg;
    public GameObject wallText;
    public Text itemText;
    private Rigidbody2D rb;


    private void Start()
    {
        myAnimator.SetBool("move", false);
        rb = GetComponent<Rigidbody2D>();
        startPos = new Vector3(-6.95f, -2.7f, 0f);
        playerImg.sprite = pImg[0];
    }

    void Update()
    {
        #region 이동
        // 방향 입력
        float direction = Input.GetAxis("Horizontal");

        // 방향에 따른 스케일 수정 및 애니메이션 상태 변경
        if (direction != 0)
        {
            // 현재 방향에 따라 스케일을 수정하고, 애니메이션 상태 변경
            if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
            {
                // 반대 방향으로 돌릴 때만 스케일을 수정
                transform.localScale = new Vector3(Mathf.Sign(direction) * 7f, 7f, 1);
                isFacingRight = !isFacingRight; // 방향 전환 상태 업데이트
            }
            myAnimator.SetBool("move", true); // 이동 중인 상태
        }
        else
        {
            myAnimator.SetBool("move", false); // 정지 상태
        }

        // 점프 입력 처리
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isJumping)
        {
            Jump();
        }

        // 캐릭터 이동 (Rigidbody2D의 velocity로 이동 처리)
        Vector2 currentVelocity = rb.velocity;
        rb.velocity = new Vector2(direction * speed, currentVelocity.y); // 수평 이동만 업데이트



        #endregion

        #region 공격
        if (Input.GetKeyDown(KeyCode.Z))
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

        playerHealthSlider.value = DataBaseManager.Instance.playerHealth;
        skillSlider.value = skillDamage;

        if (Input.GetKey(KeyCode.X) && skillDamage <= 10)
        {
            skillSlider.gameObject.SetActive(true);
            skillDamage += Time.deltaTime;
            myAnimator.SetBool("isReady", true);
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            myAnimator.SetBool("isReady", false);
            skillSlider.gameObject.SetActive(false);
            skillDamage = 0;

        }
        #endregion

        #region HP
        if (DataBaseManager.Instance.playerHealth <= 0)
        {
            SceneManager.LoadScene("LobbyScene");
        }
        #endregion

        
    }



    #region 닿는 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // "Ground" 태그를 가진 객체와 충돌 시
        if (collision.collider.CompareTag("Ground"))
        {
            isJumping = false;
        }

        // "Wall" 태그를 가진 객체와 충돌 시
        if (collision.collider.CompareTag("Wall"))
        {
            if (wallText != null)
                wallText.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // "Wall" 태그를 가진 객체와 충돌을 벗어날 때
        if (collision.collider.CompareTag("Wall"))
        {
            if (wallText != null)
                wallText.SetActive(false);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region 태그
        if (collision.CompareTag("Portal")) // 포탈 태그 감지
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // 현재 씬 인덱스 가져오기
            int nextSceneIndex = currentSceneIndex + 1; // 다음 씬

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) // 씬이 남아있다면
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else // 마지막 씬이면 처음으로 돌아감
            {
                SceneManager.LoadScene(0);
            }
        }

        //outline
        if (collision.CompareTag("OutLine"))
        {
            gameObject.transform.position = startPos;
        }

        //아이템
        if (collision.CompareTag("SpeedItem"))
        {
            speed += 10;
            Invoke("SpeedUp", 5f);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Invincible"))
        {
            Invincible = true;
            Invoke("InvincibleUp", 5f);
            itemText.text = "무적이다냥";
            itemText.gameObject.SetActive(true);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("JumpItem"))
        {
            jumpForce += 2;
            Invoke("JumpUp", 5f);
            Destroy(collision.gameObject);
        }

        //장애물
        if (collision.CompareTag("disabled"))
        {
            TakeDamage(damage_);

            if (rb != null)
            {
                Vector2 knockbackDirection = (transform.position.x > collision.transform.position.x) ?
                new Vector2(1, 1) : new Vector2(-1, 1); // 무조건 대각선 방향 (좌상 or 우상)

                knockbackDirection.Normalize();

                float knockbackForce = 5.5f;
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
        #endregion

    }

    #endregion

    #region 점프
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
            DataBaseManager.Instance.playerHealth -= damage;
        }
    }
    #endregion

    #region 아이템
    public void SpeedUp()
    {
        speed -= 10;
    }
    void InvincibleUp()
    {
        Invincible = false;
        itemText.gameObject.SetActive(false);
    }
    void JumpUp()
    {
        jumpForce -= 2;
    }
    #endregion 
}
