using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{

    public AudioClip clickSound;
    public GameObject Panelpop;
    private AudioSource audioSources;
    public DataBaseManager dataBaseManager;

    private void Start()
    {
        audioSources = GetComponent<AudioSource>();
        dataBaseManager.Init();
    }
    public void GameStart()
    {
        SceneManager.LoadScene("Start_StoryScene");
        dataBaseManager.playerHealth = 100;
        audioSources.PlayOneShot(clickSound);
    }

    public void Lobby()
    {
        SceneManager.LoadScene("LobbyScene");
        audioSources.PlayOneShot(clickSound);
    }

    public void GameExit()
    {
        audioSources.PlayOneShot(clickSound);
        Application.Quit();
    }

    public void PanelOpen()
    {
        Panelpop.SetActive(true);
        audioSources.PlayOneShot(clickSound);
    }

    public void Paneldown()
    {
        Panelpop.SetActive(false);
        audioSources.PlayOneShot(clickSound);
    }
}
