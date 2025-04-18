using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimelineEndHandler : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    public string nextSceneName; // ´ÙÀ½ ¾À ÀÌ¸§

    public Image reloadingImg;
    public FadeManager fadeManager;
   

    void Start()
    {
        if (timelineDirector != null)
            timelineDirector.stopped += OnTimelineStopped;
        fadeManager.FadeIn();
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

        if (reloadingImg.fillAmount >= 1)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
    void OnTimelineStopped(PlayableDirector director)
    {
        fadeManager.FadeOut();
        Invoke("FadeSceneMove", 1.5f);
        Boss.Instance.BossDeath();

    }

    void FadeSceneMove()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
