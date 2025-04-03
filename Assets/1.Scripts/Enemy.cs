using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;          // �̵� �ӵ�
    public float moveDistance = 5f;   // �̵� �Ÿ�

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private int direction = 1;        // �̵� ���� (1: ������, -1: ����)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
    }

    void FixedUpdate()
    {
        // ���ο� ��ġ ���
        Vector2 targetPosition = rb.position + Vector2.right * speed * direction * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);

        // Ư�� �Ÿ� �̻� �̵��ϸ� ���� ����
        if (Mathf.Abs(rb.position.x - startPosition.x) >= moveDistance)
        {
            direction *= -1; // ���� ����
            FlipSprite();    // ��������Ʈ ������
        }
    }

    void FlipSprite()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // X�� ���� ����
        transform.localScale = localScale;
    }
}
