using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrap : MonoBehaviour
{

    [SerializeField] private ParticleSystem particleSystemBlood = null;
    private float timeBlood = 0f;
    private bool onBlood = false;

    private void Start()
    {
        particleSystemBlood.Play();
        particleSystemBlood.Stop();
    }

    public void ActiveBlood(float duration)
    {
        onBlood = true;
        timeBlood = duration;
        particleSystemBlood.Play();
    }

    private void Update()
    {
        if (onBlood)
        {
            timeBlood -= Time.deltaTime;
            if (timeBlood <= 0f)
            {
                onBlood = false;
                particleSystemBlood.Stop();
            }
        }
    }

}
