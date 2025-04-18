using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimelineEndHandler : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    public string nextSceneName; // 다음 씬 이름

    public Image reloadingImg;

    void Start()
    {
        if (timelineDirector != null)
            timelineDirector.stopped += OnTimelineStopped;
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            reloadingImg.fillAmount += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            reloadingImg.fillAmount = 0;
        }

        if(reloadingImg.fillAmount >= 1)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
    void OnTimelineStopped(PlayableDirector director)
    {
        Debug.Log("타임라인 끝났으니 씬 이동!");
        SceneManager.LoadScene(nextSceneName);
    }
}
