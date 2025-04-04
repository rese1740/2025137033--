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
    private float damage_ = 5.0f;

    [Header("체력")]
    public Slider playerHealthSlider;
    private bool Invincible = false;


    [Header("기타")]
    public Animator myAnimator;
    public Image playerImg;
    public Sprite[] pImg;
    public GameObject wallText; 
    private int ImgIndex;
    private GameObject ImgObject0, ImgObject1, ImgObject2;
    private Rigidbody2D rb;


    private void Start()
    {
        myAnimator.SetBool("move", false);
        rb = GetComponent<Rigidbody2D>();
        playerImg.sprite = pImg[0];
    }

    void Update()
    {
        #region 이동
        float direction = Input.GetAxis("Horizontal");

        // 방향에 따른 스케일 수정 및 애니메이션 상태 변경
        if (direction != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction) * 7f, 7f, 1); // 방향에 맞춰 스케일을 수정
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


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isJumping)
            {
                Jump();
            }
        }
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        #endregion

        #region 포탈
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(ImgObject0 != null)
            {
                playerImg.sprite = pImg[0];
            }
            if (ImgObject1 != null)
            {
                playerImg.sprite = pImg[1];
            }
            if (ImgObject2 != null)
            {
                playerImg.sprite = pImg[2];
            }
        }
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


        //UI 이미지
        if (collision.CompareTag("Item0"))
        {
            ImgObject0 = collision.gameObject;
        }
        else if (collision.CompareTag("Item1"))
        {
            ImgObject1 = collision.gameObject;
        }
        else if (collision.CompareTag("Item2"))
        {
            ImgObject2 = collision.gameObject;
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
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("JumpItem"))
        {
            Invincible = true;
            Invoke("InvincibleUp", 5f);
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        //UI 이미지
        if (collision.CompareTag("Item0"))
        {
            if (collision.gameObject == ImgObject0)
            {
                ImgObject0 = null;
            }
        }
        else if (collision.CompareTag("Item1"))
        {
            if (collision.gameObject == ImgObject1)
            {
                ImgObject1 = null;
            }
        }
        else if (collision.CompareTag("Item2"))
        {
            if (collision.gameObject == ImgObject2)
            {
                ImgObject2 = null;
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
            DataBaseManager.Instance.playerHealth -= damage;
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
