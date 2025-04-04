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
   

    [Header("����")]
    public GameObject bullet_;
    public Transform gunPos;
    private bool isFacingRight = true;
    private float damage_ = 5.0f;

    [Header("ü��")]
    public Slider playerHealthSlider;
    private bool Invincible = false;


    [Header("��Ÿ")]
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
        #region �̵�
        float direction = Input.GetAxis("Horizontal");

        // ���⿡ ���� ������ ���� �� �ִϸ��̼� ���� ����
        if (direction != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction) * 7f, 7f, 1); // ���⿡ ���� �������� ����
            myAnimator.SetBool("move", true);  // �̵� ���� ����
        }
        else
        {
            myAnimator.SetBool("move", false);  // ���� ����
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

        #region ��Ż
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


        //UI �̹���
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
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("JumpItem"))
        {
            Invincible = true;
            Invoke("InvincibleUp", 5f);
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        //UI �̹���
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
