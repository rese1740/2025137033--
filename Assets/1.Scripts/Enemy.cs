using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;          // 이동 속도
    public float moveDistance = 5f;   // 이동 거리

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private int direction = 1;        // 이동 방향 (1: 오른쪽, -1: 왼쪽)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
    }

    void FixedUpdate()
    {
        // 새로운 위치 계산
        Vector2 targetPosition = rb.position + Vector2.right * speed * direction * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);

        // 특정 거리 이상 이동하면 방향 반전
        if (Mathf.Abs(rb.position.x - startPosition.x) >= moveDistance)
        {
            direction *= -1; // 방향 반전
            FlipSprite();    // 스프라이트 뒤집기
        }
    }

    void FlipSprite()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // X축 방향 반전
        transform.localScale = localScale;
    }
}
