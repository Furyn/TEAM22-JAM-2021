using UnityEngine;

public class Event : MonoBehaviour
{
    [SerializeField] private string Name = "Event Name";
    [SerializeField] private float timerDuration = 10f;
    internal Transform startTerainPosition = null;
    internal Transform endTerainPosition = null;

    public bool eventLaunch = false;
    private float timer = 0f;

    public virtual void SetUpPostion(Transform start, Transform end)
    {
        startTerainPosition = start;
        endTerainPosition = end;
    }

    public virtual void Apply()
    {
        eventLaunch = true;
        timer = timerDuration;
    }

    public virtual void Stop()
    {
        eventLaunch = false;
        timer = 0f;
    }

    public virtual void UpdateEvent()
    {
        if (eventLaunch)
        {
            Debug.Log(timer);
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Stop();
            }
        }
    }

    private void Update()
    {
        UpdateEvent();
    }
}
