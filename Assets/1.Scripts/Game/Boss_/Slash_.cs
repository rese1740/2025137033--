using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash_ : MonoBehaviour
{
    public float death_Timer = 0;   
    void Start()
    {
        Destroy(gameObject,death_Timer);
    }
}
