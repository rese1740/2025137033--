using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{

    [Header("�̵�")]
    public int speed = 5;
    public float jumpForce = 100f;
    public float bulletSpeed = 10f;
    public bool isJumping = false;
    private Vector3 startPos;


    [Header("����")]
    public GameObject bullet_;
    public Transform gunPos;
    private bool isFacingRight = true;
    private float damage_ = 5.0f;
    //��ų 1
    public float skillDamage = 0;
    public Slider skillSlider;


    [Header("ü��")]
    public Slider playerHealthSlider;
    private bool Invincible = false;


    [Header("��Ÿ")]
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
        #region �̵�
        // ���� �Է�
        float direction = Input.GetAxis("Horizontal");

        // ���⿡ ���� ������ ���� �� �ִϸ��̼� ���� ����
        if (direction != 0)
        {
            // ���� ���⿡ ���� �������� �����ϰ�, �ִϸ��̼� ���� ����
            if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
            {
                // �ݴ� �������� ���� ���� �������� ����
                transform.localScale = new Vector3(Mathf.Sign(direction) * 7f, 7f, 1);
                isFacingRight = !isFacingRight; // ���� ��ȯ ���� ������Ʈ
            }
            myAnimator.SetBool("move", true); // �̵� ���� ����
        }
        else
        {
            myAnimator.SetBool("move", false); // ���� ����
        }

        // ���� �Է� ó��
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isJumping)
        {
            Jump();
        }

        // ĳ���� �̵� (Rigidbody2D�� velocity�� �̵� ó��)
        Vector2 currentVelocity = rb.velocity;
        rb.velocity = new Vector2(direction * speed, currentVelocity.y); // ���� �̵��� ������Ʈ



        #endregion

        #region ����
        if (Input.GetKeyDown(KeyCode.Z))
        {
            myAnimator.SetTrigger("attack");

            Vector2 shootDirection = isFacingRight ? Vector2.right : Vector2.left;  // �������̸� (1, 0), �����̸� (-1, 0)

            // �Ѿ� ����
            GameObject bullet = Instantiate(bullet_, gunPos.position, Quaternion.identity);

            // �Ѿ��� ���� ����
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(shootDirection);
            }

            // �Ѿ� �ӵ� ����
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



    #region ��� ó��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // "Ground" �±׸� ���� ��ü�� �浹 ��
        if (collision.collider.CompareTag("Ground"))
        {
            isJumping = false;
        }

        // "Wall" �±׸� ���� ��ü�� �浹 ��
        if (collision.collider.CompareTag("Wall"))
        {
            if (wallText != null)
                wallText.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // "Wall" �±׸� ���� ��ü�� �浹�� ��� ��
        if (collision.collider.CompareTag("Wall"))
        {
            if (wallText != null)
                wallText.SetActive(false);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region �±�
        if (collision.CompareTag("Portal")) // ��Ż �±� ����
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // ���� �� �ε��� ��������
            int nextSceneIndex = currentSceneIndex + 1; // ���� ��

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) // ���� �����ִٸ�
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else // ������ ���̸� ó������ ���ư�
            {
                SceneManager.LoadScene(0);
            }
        }

        //outline
        if (collision.CompareTag("OutLine"))
        {
            gameObject.transform.position = startPos;
        }

        //������
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
            itemText.text = "�����̴ٳ�";
            itemText.gameObject.SetActive(true);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("JumpItem"))
        {
            jumpForce += 2;
            Invoke("JumpUp", 5f);
            Destroy(collision.gameObject);
        }

        //��ֹ�
        if (collision.CompareTag("disabled"))
        {
            TakeDamage(damage_);

            if (rb != null)
            {
                Vector2 knockbackDirection = (transform.position.x > collision.transform.position.x) ?
                new Vector2(1, 1) : new Vector2(-1, 1); // ������ �밢�� ���� (�»� or ���)

                knockbackDirection.Normalize();

                float knockbackForce = 5.5f;
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
        #endregion

    }

    #endregion

    #region ����
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

    #region ������
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
