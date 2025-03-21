using UnityEngine;

public class SimpleMoveController : MonoBehaviour
{
    public Animator myAnimator;
    public int speed = 5;

    private void Start()
    {
        myAnimator.SetBool("move", false);
    }

    void Update()
    {
        float direction = Input.GetAxis("Horizontal");

        if (direction > 0)
        {
            transform.localScale = new Vector3(0.8f, 0.8f, 1);
            myAnimator.SetBool("move", true);
        }
        else if (direction < 0)
        {
            transform.localScale = new Vector3(-0.8f, 0.8f, 1);
            myAnimator.SetBool("move", true);
        }
        else
        {
            myAnimator.SetBool("move", false);
        }

        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
    }
}
