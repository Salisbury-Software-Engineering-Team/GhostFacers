using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public GameObject MenuUI;
    public GameObject QuitUI;

    private void Start()
    {
        MenuUI.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!MenuUI.activeInHierarchy)
                MenuUI.SetActive(true);
            else
            {
                MenuUI.SetActive(false);
                QuitUI.SetActive(false);
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
