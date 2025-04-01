using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    private Vector2 direction;

    // 방향을 설정하는 함수
    public void SetDirection(Vector2 direction)
    {
        this.direction = direction; // 총알의 이동 방향 설정
    }

    private void Start()
    {
        Invoke("bulletDestroy", 3f);
    }
    void Update()
    {
        transform.Translate(direction * bulletSpeed * Time.deltaTime);
    }

    public void bulletDestroy()
    {
        Destroy(gameObject);
    }
}
