using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    private Vector2 direction;

    // ������ �����ϴ� �Լ�
    public void SetDirection(Vector2 direction)
    {
        this.direction = direction; // �Ѿ��� �̵� ���� ����
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
