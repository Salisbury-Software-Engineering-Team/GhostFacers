using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannotAttackMessage : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float seconds = .5f;


    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        StartCoroutine(DisplayForSeconds());
    }

    private IEnumerator DisplayForSeconds()
    {
        CanvasGroup panel = transform.GetComponent<CanvasGroup>();
        yield return new WaitForSeconds(seconds);
        while (panel.alpha > 0)
        {
            panel.alpha -= Time.deltaTime * speed;
            yield return null;
        }
        gameObject.SetActive(false);
        panel.alpha = 1;
    }

}
