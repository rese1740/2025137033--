using UnityEngine;

public class EnemyTraceController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float raycastDistance = 0.5f;
    public float traceDistance = 5f;

    private Transform player;
    private Rigidbody2D rb;
    private bool lostPlayer = false; // 플레이어를 잃었는지 체크

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 direction = player.position - transform.position;

        // 플레이어가 너무 멀면 이동하지 않음
        if (direction.magnitude > traceDistance)
        {
            rb.velocity = Vector2.zero; // 멈춤
            lostPlayer = true;
            return;
        }

        Vector2 directionNormalized = direction.normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionNormalized, raycastDistance);
        Debug.DrawRay(transform.position, directionNormalized * raycastDistance, Color.red);

        // 이동 적용
        rb.velocity = directionNormalized * moveSpeed;
        lostPlayer = false; // 플레이어를 다시 찾음
    }
   

}
