using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public GameObject Player;
    public Transform PlayerSpawnTr;
    public GameObject Esc_UI;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Esc_UI.SetActive(!Esc_UI.activeSelf);
        }
    }

    

}
