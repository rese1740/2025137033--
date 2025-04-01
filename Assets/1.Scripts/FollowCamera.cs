using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    

    float cameraoffset = -10.0f;

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y + 3.0f, cameraoffset);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
    }
}
