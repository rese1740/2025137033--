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

    [Header("ü��")]
    public float playerHealth = 10.0f;
    public Slider playerHealthSlider;
    private bool Invincible = false;


    [Header("��Ÿ")]
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

        // ���⿡ ���� ������ ���� �� �ִϸ��̼� ���� ����
        if (direction != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction) * 10f, 10f, 1); // ���⿡ ���� �������� ����
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

        if (Input.GetMouseButtonDown(0)) // ���콺 ��Ŭ���� ������
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

        playerHealthSlider.value = playerHealth;

        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
    }


    #region ��� ó��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region �±�
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
