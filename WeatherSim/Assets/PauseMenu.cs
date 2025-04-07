using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pause(){
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
    public void resume(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
