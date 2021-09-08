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
        Debug.Log(name + " was launched");
    }
    public virtual void Stop()
    {
        eventLaunch = false;
        timer = 0f;
        Debug.Log("STOP");
    }

    private void Update()
    {
        if (eventLaunch)
        {
            timer -= Time.deltaTime;
            Debug.Log("UPDATE");
            if (timer <= 0f)
            {
                Stop();
            }
        }
    }
}
