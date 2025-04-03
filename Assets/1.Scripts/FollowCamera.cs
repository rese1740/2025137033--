using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    float cameraoffset = -10.0f;
    public float FollowSpeed = 5.0f;
    public float CameraPosY;

    public float minX = -5.0f;
    public float maxX = 10.0f;
    public float minY = -5.0f;  // Add min Y limit
    public float maxY = 5.0f;   // Add max Y limit

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y + CameraPosY, cameraoffset);

        // X 값 제한 적용
        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);

        // Y 값 제한 적용
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

        transform.position = Vector3.Lerp(transform.position, targetPos, FollowSpeed * Time.deltaTime);
    }
}
