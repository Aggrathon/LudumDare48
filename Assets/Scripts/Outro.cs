using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class Outro : MonoBehaviour
{

    public TextMeshProUGUI text;
    public GameObject otherUI;

    public void Show(string description)
    {
        text.text = description;
        gameObject.SetActive(true);
        otherUI.SetActive(false);
        GetComponent<AudioSource>().Play();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
