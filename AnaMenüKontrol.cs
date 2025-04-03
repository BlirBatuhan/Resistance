using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnaMenüKontrol : MonoBehaviour
{
    public GameObject EminmisinPanel;

    void Start()
    {

    }

    public void OyunaBasla()
    {
        SceneManager.LoadScene(1);
    }
    public void Cıkıs()
    {
        EminmisinPanel.SetActive(true);
    }

    public void CıkısCevap(string cevap)
    {
        switch (cevap)
        {
            case "evet":
                Application.Quit();
                break;
            case "hayır":
                EminmisinPanel.SetActive(false);
                break;
        }
    }
}

