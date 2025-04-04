using UnityEngine;

public class EnemyTraceController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float raycastDistance = 0.5f;
    public float traceDistance = 5f;

    private Transform player;
    private Rigidbody2D rb;
    private bool lostPlayer = false; // �÷��̾ �Ҿ����� üũ

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 direction = player.position - transform.position;

        // �÷��̾ �ʹ� �ָ� �̵����� ����
        if (direction.magnitude > traceDistance)
        {
            rb.velocity = Vector2.zero; // ����
            lostPlayer = true;
            return;
        }

        Vector2 directionNormalized = direction.normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionNormalized, raycastDistance);
        Debug.DrawRay(transform.position, directionNormalized * raycastDistance, Color.red);

        // �̵� ����
        rb.velocity = directionNormalized * moveSpeed;
        lostPlayer = false; // �÷��̾ �ٽ� ã��
    }
   

}
