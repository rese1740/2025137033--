using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float spawnInterval = 0.05f;
    private float timer;

    public bool isGhosting = false;

    private SpriteRenderer playerSprite;

    void Start()
    {
        playerSprite = GetComponentInParent<SpriteRenderer>();
        if (playerSprite == null)
        {
            Debug.LogError("�÷��̾ SpriteRenderer�� �����ϴ�!");
        }
    }

    void Update()
    {
        if (!isGhosting || playerSprite == null) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnGhost();
            timer = spawnInterval;
        }
    }

    void SpawnGhost()
    {
        GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        SpriteRenderer sr = ghost.GetComponent<SpriteRenderer>();

        sr.sprite = playerSprite.sprite;
        sr.flipX = playerSprite.flipX;
        ghost.transform.localScale = transform.localScale;

        Destroy(ghost, 0.5f);
    }
}
