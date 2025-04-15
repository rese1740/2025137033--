using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DataBaseManager : ScriptableObject
{
    public static DataBaseManager Instance;

    [Header("ü��")]
    public float playerHealth = 10.0f;

    

    public void Init()
    {
        Instance = this;
    }

  
}
