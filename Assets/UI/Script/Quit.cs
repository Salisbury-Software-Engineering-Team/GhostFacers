using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public GameObject QuitUI;

    private void Start()
    {
        QuitUI.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!QuitUI.activeInHierarchy)
                QuitUI.SetActive(true);
            else
                QuitUI.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
