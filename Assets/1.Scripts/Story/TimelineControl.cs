using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineEndHandler : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    public string nextSceneName; // 다음 씬 이름

    void Start()
    {
        if (timelineDirector != null)
            timelineDirector.stopped += OnTimelineStopped;
    }

    void OnTimelineStopped(PlayableDirector director)
    {
        Debug.Log("타임라인 끝났으니 씬 이동!");
        SceneManager.LoadScene(nextSceneName);
    }
}
