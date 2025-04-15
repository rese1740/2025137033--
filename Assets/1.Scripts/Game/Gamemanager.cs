using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        #region ����������
        if (Input.GetKeyDown(KeyCode.F5))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // ���� �� �ε��� ��������
            int nextSceneIndex = currentSceneIndex + 1; // ���� ��

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) // ���� �����ִٸ�
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else // ������ ���̸� ó������ ���ư�
            {
                SceneManager.LoadScene(0);
            }
        }

        #endregion
    }



}
