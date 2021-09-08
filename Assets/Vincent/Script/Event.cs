using UnityEngine;

public class Event : MonoBehaviour
{
    [SerializeField] private string Name = "Event Name";
    public bool eventLaunch = false;
    [SerializeField] private float timerDuration = 10f;
    private float timer = 0f;

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

    private void Update()
    {
        if (eventLaunch)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Stop();
            }
        }
    }
}
