using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject GameOverPanel;
    public GameObject GameWinPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void Kazandin()
    {
       GameWinPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
    public void Kaybettin()
    {
        Cursor.lockState = CursorLockMode.None;
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void YenidenOyna()
    {

        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void AnaMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
