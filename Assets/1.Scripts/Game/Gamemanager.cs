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

        #region 개발자전용
        if (Input.GetKeyDown(KeyCode.F5))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // 현재 씬 인덱스 가져오기
            int nextSceneIndex = currentSceneIndex + 1; // 다음 씬

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) // 씬이 남아있다면
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else // 마지막 씬이면 처음으로 돌아감
            {
                SceneManager.LoadScene(0);
            }
        }

        #endregion
    }



}
