using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutoScreen : MonoBehaviour
{
    public GameObject context1;
    public GameObject context2;
    public GameObject rules;
    public GameObject controls;
    public Text nextTxt;
    private int step;

    // Start is called before the first frame update
    void Start()
    {
        context1.SetActive(true);
        step = 0;
    }

    // Update is called once per frame
    public void OnNext()
    {
        if (step == 0)
        {
            context1.SetActive(false);
            context2.SetActive(true);
            step++;
        }

        else if (step == 1)
        {
            context2.SetActive(false);
            rules.SetActive(true);
            step++;
        }

        else if (step == 2)
        {
            rules.SetActive(false);
            controls.SetActive(true);
            step++;
            nextTxt.text = "Play";
        }

        else if (step == 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
