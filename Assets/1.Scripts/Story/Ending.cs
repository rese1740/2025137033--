using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    private Animator myAnim;
    public Transform _camera;


    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(_camera.position.x >= 296f)
        {
            myAnim.SetTrigger("isStrech");
            Invoke("SleepAnim", 1f);
        }
    }

    void SleepAnim()
    {
        myAnim.SetTrigger("isSleep");
    }


}
