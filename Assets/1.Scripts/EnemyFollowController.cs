using UnityEngine;

public class EnemyFollowController : MonoBehaviour
{
    public float moveSpeed = 3f; // ���� �̵� �ӵ�
    private Transform player; // �÷��̾��� ��ġ
    private SpriteRenderer spriteRenderer; // ���� ��������Ʈ ������

    private void Start()
    {
        // "Player" �±׸� ���� ��ü�� ã�� player ������ �Ҵ�
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>(); // ���� ��������Ʈ ������ ������Ʈ�� ������
    }

    private void Update()
    {
        if (player != null)
        {
            // �÷��̾��� ��ġ�� ���� �̵���Ŵ
            Vector3 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            if (direction.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }
}
