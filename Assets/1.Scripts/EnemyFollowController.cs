using UnityEngine;

public class EnemyFollowController : MonoBehaviour
{
    public float moveSpeed = 3f; // 적의 이동 속도
    private Transform player; // 플레이어의 위치
    private SpriteRenderer spriteRenderer; // 적의 스프라이트 렌더러

    private void Start()
    {
        // "Player" 태그를 가진 객체를 찾아 player 변수에 할당
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>(); // 적의 스프라이트 렌더러 컴포넌트를 가져옴
    }

    private void Update()
    {
        if (player != null)
        {
            // 플레이어의 위치로 적을 이동시킴
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
