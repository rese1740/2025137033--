using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineEndHandler : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    public string nextSceneName; // ���� �� �̸�

    void Start()
    {
        if (timelineDirector != null)
            timelineDirector.stopped += OnTimelineStopped;
    }

    void OnTimelineStopped(PlayableDirector director)
    {
        Debug.Log("Ÿ�Ӷ��� �������� �� �̵�!");
        SceneManager.LoadScene(nextSceneName);
    }
}
