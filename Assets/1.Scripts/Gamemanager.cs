using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public GameObject Esc_UI;
    public DataBaseManager dataBaseManager;

    private void Start()
    {
        dataBaseManager.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Esc_UI.SetActive(!Esc_UI.activeSelf);
        }
    }

    

}
