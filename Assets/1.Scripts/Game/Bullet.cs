using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    private Vector2 direction = Vector2.right;  // 기본값: 오른쪽

    private Rigidbody2D rb;

    public void SetDirection(Vector2 dir)
    {
        if (dir != Vector2.zero)
            direction = dir.normalized;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = direction * bulletSpeed;
        Invoke(nameof(DestroyBullet), 3f);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
